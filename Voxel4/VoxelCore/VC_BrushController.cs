using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Voxel4.Internal;
using System;

namespace Voxel4
{
    /*
     ===== Brush Controller =====
    2 jobs:
    - handle the preview of the brush
    - writes the voxels using VC_VoxelWriter

    Painting is done the following way:
    - we internaly write to a chunk net the given pattern. Always centered on (0,0,0)
    - depending on the paint action, we copy/nullify values on the VoxelCore based
      on that internal pattern + a snapped offset value.
     */
    public partial class VoxelCore
    {
        class BrushController
        {
            VoxelCore _vc;
            ChunkNet _internalNet = new ChunkNet();
            // brush position + offset setting (snapped since it is a V3I)

            Vector3Int _finalOffset = Vector3Int.zero;
            Quaternion _brushRot = Quaternion.identity;

            public BrushController(VoxelCore vc)
            {
                _vc = vc;
            }

            public void Paint()
            {
                switch(_vc.Tool.Action)
                {
                    case ToolAction.Write:
                        paintInWriteMode();
                        break;
                    case ToolAction.Erase:
                        paintInEraseMode();
                        break;
                }
            }

            void paintInWriteMode()
            {
                foreach ((int, int, int, VoxelData) vx in _internalNet.Voxels)
                {
                    if (vx.Item4 != null)
                    {
                        // Debug.Log($"fo: {_finalOffset}; vx:{vx.Item1} {vx.Item2} {vx.Item3}");
                        _vc._voxelWriter.WriteVoxel(
                            vx.Item1 + _finalOffset.x,
                            vx.Item2 + _finalOffset.y,
                            vx.Item3 + _finalOffset.z,
                            vx.Item4.Color);
                    }
                }
            }

            void paintInEraseMode()
            {
                foreach ((int, int, int, VoxelData) vx in _internalNet.Voxels)
                {
                    if (vx.Item4 != null)
                    {
                        _vc._voxelWriter.EraseVoxel(
                            vx.Item1 + _finalOffset.x,
                            vx.Item2 + _finalOffset.y,
                            vx.Item3 + _finalOffset.z);
                    }
                }
            }

            public void FrameUpdate()
            {
                // compute new final offset and rotation
                _brushRot = _vc.BrushDeviceTransform.rotation;
                // Currently working version:
                // -----
                /*
                _finalOffset = Vector3Int.RoundToInt(
                    _vc.BrushDeviceTransform.position +
                    _brushRot * (Vector3.forward * _vc.Tool.BrushOffset));
                */
                // -----
                // test (seams to be working):
                _finalOffset = Vector3Int.RoundToInt(
                    WorldToVoxelScaling(
                        _vc.BrushDeviceTransform.position +
                        _brushRot * (Vector3.forward * _vc.Tool.BrushOffset)
                        ));

                // Debug.Log($"fo:{_finalOffset} | ro:{Vector3Int.RoundToInt(_vc.BrushDeviceTransform.position)} | o:{_vc.BrushDeviceTransform.position}");

                // clear internal net
                _internalNet.Net.ClearChunkContent();

                // regen internal net
                switch(_vc.Tool.Shape)
                {
                    case ToolShape.UnitCube:
                        internalPaintUnitCube();
                        break;
                    case ToolShape.Cube:
                        internalPaintCube();
                        break;
                    case ToolShape.RotatedCube:
                        internalPaintRotatedCube();
                        break;
                    case ToolShape.Line:
                        internalPaintLine();
                        break;
                    case ToolShape.Sphere:
                        // throw new NotImplementedException("Sphere");
                        internalPaintSphere();
                        break;
                    case ToolShape.RotatedSphere:
                        // throw new NotImplementedException("Sphere");
                        internalPaintRotatedSphere();
                        break;
                }

                // Meshing
                foreach(Chunk chunk in _internalNet.chunks.Values)
                {
                    _vc._chunkMeshing.MeshChunk(chunk);
                    // chunk.IsActive = true;
                }

                // place internal net at brush + offset
                // internal (0,0,0) = (_offset.x, _offset.y, _offset.z)
                // so we get the would be chunk at offset_x/y/z
                // this is a 'chunkOffset'
                var (centerX, centerY, centerZ) =
                    ChunkNet.VoxelsView.ChunkCoordFromVoxCoord(_finalOffset.x, _finalOffset.y, _finalOffset.z);
                // To turn the internal chunk grid position into world chunk grid
                // position, we just add this chunkOffset to the internal coordinates
                // of every chunk
                foreach(KeyValuePair<(int, int, int), Chunk> kvp in _internalNet.chunks)
                {
                    Vector3 actualGridPos = new Vector3(
                        kvp.Key.Item1 + centerX,
                        kvp.Key.Item2 + centerY,
                        kvp.Key.Item3 + centerZ);

                    _vc._renderingController.UpdateChunkGridPosScaleVis(actualGridPos, kvp.Value);
                    Vector3 actualPos = VoxelToWorldScaling(actualGridPos);
                    // kvp.Value.Trs.position = actualPos;
                    // kvp.Value.Trs.position = _vc.BrushDeviceTransform.position;
                    // kvp.Value.Trs.position = _finalOffset;

                    // kvp.Value.Trs.position = VoxelToWorldScaling(_finalOffset);

                    // CURRENTLY WORKING VERSION
                    // -----
                    //kvp.Value.Trs.position = (16 * new Vector3(kvp.Key.Item1, kvp.Key.Item2, kvp.Key.Item3)) + VoxelToWorldScaling(_finalOffset);
                    // -----
                    // test (seams to work):
                    kvp.Value.Trs.position = (_vc._voxelSize * 16 * new Vector3(kvp.Key.Item1, kvp.Key.Item2, kvp.Key.Item3)) + VoxelToWorldScaling(_finalOffset);

                    // Debug.Log($"bp: {_vc.BrushDeviceTransform.position} | fo: {_finalOffset}");
                }
            }

            void internalVoxPaint(int x, int y, int z)
            {
                switch(_vc.Tool.Action)
                {
                    case ToolAction.Write:
                        _internalNet.Voxels[x, y, z] = new VoxelData(_vc.Tool.Color);
                        break;
                    case ToolAction.Erase:
                        if (_vc.Tool.Color == Color.white)
                        {
                            _internalNet.Voxels[x, y, z] = new VoxelData(Color.black);
                        }
                        else
                        {
                            _internalNet.Voxels[x, y, z] = new VoxelData(Color.white);
                        }
                        break;
                }
            }

            void internalPaintUnitCube()
            {
                // internalVoxPaint(_finalOffset.x, _finalOffset.y, _finalOffset.z);
                internalVoxPaint(0, 0, 0);
            }

            void internalPaintCube()
            {
                int edgeLen = Math.Max(1, (int)_vc.Tool.ToolSize);
                int offset = -((edgeLen - 1) / 2);
                Common.ActOnMatrixIterate(edgeLen, edgeLen, edgeLen, (x, y, z) =>
                {

                    /*
                    Vector3 point = new Vector3(x - offset, y - offset, z - offset);
                    Vector3Int snappedRotatedPoint = Vector3Int.RoundToInt(
                        _brushRot * point);
                    internalVoxPaint(snappedRotatedPoint.x, snappedRotatedPoint.y, snappedRotatedPoint.z);
                    */

                    internalVoxPaint(x + offset, y + offset, z + offset);

                    // Debug.Log($"x:{x + offset} y:{y + offset} z:{z + offset}");
                });
            }

            void internalPaintRotatedCube()
            {
                int edgeLen = Math.Max(1, (int)_vc.Tool.ToolSize);
                int offset = -((edgeLen - 1) / 2);
                Common.ActOnMatrixIterate(edgeLen, edgeLen, edgeLen, (x, y, z) =>
                {

                    Vector3 point = new Vector3(x + offset, y + offset, z + offset);
                    Vector3Int snappedRotatedPoint = Vector3Int.RoundToInt(
                        _brushRot * point);
                    internalVoxPaint(snappedRotatedPoint.x, snappedRotatedPoint.y, snappedRotatedPoint.z);

                    // internalVoxPaint(x + offset, y + offset, z + offset);

                    // Debug.Log($"x:{x + offset} y:{y + offset} z:{z + offset}");
                });
            }

            void internalPaintLine()
            {
                int edgeLen = Math.Max(1, (int)_vc.Tool.ToolSize);
                // forward is 0,0,1
                for (int z = 0; z < edgeLen; z++)
                {
                    // transform point location based on controller's rotation
                    // and snap it to grid.
                    Vector3 point = new Vector3(0, 0, z);
                    Vector3Int snappedRotatedPoint = Vector3Int.RoundToInt(
                        _brushRot * point);

                    internalVoxPaint(snappedRotatedPoint.x, snappedRotatedPoint.y, snappedRotatedPoint.z);

                    /*
                    _brushNet.Voxels[snappedRotatedPoint.x,
                        snappedRotatedPoint.y, snappedRotatedPoint.z] = new VoxelData(_vc._voxelTool.Color);
                    */
                }
            }

            void internalPaintSphere()
            {
                // Math remainder, equation of a filled sphere centered on (0,0,0):
                // x2 + y2 + z2 <= r2

                // we know the sphere is contained in a box of dimensions 2*radius.
                // We know the following points will be part of the sphere:
                // (-radius, 0, 0) (+radius, 0, 0)
                // (0, -radius, 0) (0, +radius, 0)
                // (0, 0, -radius) (0, 0, +radius)

                // We are going to make things easier by making sure the sphere
                // is also centered on (0, 0, 0)
                // which means, keeping the radius uneven:
                int radius = (int)Mathf.Max(1, _vc.Tool.ToolSize);
                int radius2 = radius * radius;
                for (int z = -radius; z <= +radius; z++)
                {
                    for (int y = -radius; y <= +radius; y++)
                    {
                        for (int x = -radius; x <= +radius; x++)
                        {
                            Vector3Int point = new Vector3Int(x, y, z);
                            if (point.sqrMagnitude <= radius2)
                            {
                                // internalVoxPaint(x + offset, y + offset, z + offset);
                                internalVoxPaint(point.x, point.y, point.z);
                            }
                        }
                    }
                }
            }

            void internalPaintRotatedSphere()
            {
                // Math remainder, equation of a filled sphere centered on (0,0,0):
                // x2 + y2 + z2 <= r2

                // we know the sphere is contained in a box of dimensions 2*radius.
                // We know the following points will be part of the sphere:
                // (-radius, 0, 0) (+radius, 0, 0)
                // (0, -radius, 0) (0, +radius, 0)
                // (0, 0, -radius) (0, 0, +radius)

                // We are going to make things easier by making sure the sphere
                // is also centered on (0, 0, 0)
                // which means, keeping the radius uneven:
                int radius = (int)Mathf.Max(1, _vc.Tool.ToolSize);
                int radius2 = radius * radius;
                for(int z = -radius; z <= +radius; z++)
                {
                    for(int y = -radius; y <= +radius; y++)
                    {
                        for(int x = -radius; x <= +radius; x++)
                        {
                            Vector3Int point = new Vector3Int(x, y, z);
                            Vector3Int snappedRotatedPoint = Vector3Int.RoundToInt(
                                _brushRot * point);
                            if (snappedRotatedPoint.sqrMagnitude <= radius2)
                            {
                                // internalVoxPaint(x + offset, y + offset, z + offset);
                                internalVoxPaint(
                                    snappedRotatedPoint.x,
                                    snappedRotatedPoint.y,
                                    snappedRotatedPoint.z);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Converts a vector that is at voxel-scale into world scale.
            /// IE a vector(1,1,1) if the voxel len is 2 will be converted
            /// to (2,2,2).
            /// </summary>
            /// <param name="c">vector to convert</param>
            /// <returns></returns>
            public Vector3 VoxelToWorldScaling(Vector3 c)
            {
                return c * (_vc._voxelSize / VoxelCore.ChunkMeshing.voxelSize);
            }

            /// <summary>
            /// Converts a vector that is at world-scale into voxel scale.
            /// IE a vector(2,2,2) if the voxel len is 2 will be converted
            /// to (1,1,1).
            /// </summary>
            /// <param name="c">vector to convert</param>
            /// <returns></returns>
            public Vector3 WorldToVoxelScaling(Vector3 c)
            {
                return c * (VoxelCore.ChunkMeshing.voxelSize / _vc._voxelSize);
            }
        }
    }
}