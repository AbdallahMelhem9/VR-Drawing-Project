using UnityEngine;
using System.Collections.Generic;
using Voxel4.Internal;

namespace Voxel4
{
    public partial class VoxelCore
    {
        /// <summary>
        /// Controls which chunks should be regened based on non-write events
        /// (like camera movements).
        /// Controls which chunk are visible, where they should be placed and how
        /// they should be scaled.
        /// </summary>
        class RenderingController
        {
            VoxelCore _vc;
            HashSet<Vector3Int> _modifiedChunksCoords = new HashSet<Vector3Int>();
            private bool _shouldRegenerateAndReplaceAllMeshes = false;

            public RenderingController(VoxelCore vc)
            {
                _vc = vc;
            }

            public void VoxelSizeHasChanged()
            {
                _shouldRegenerateAndReplaceAllMeshes = true;
                // Debug.Log(System.Environment.StackTrace);
            }

            /// <summary>
            /// Mark the chunk as modified, so that its mesh is
            /// regenerated later
            /// </summary>
            /// <param name="x">the chunk's x coordinate</param>
            /// <param name="y">the chunk's y coordinate</param>
            /// <param name="z">the chunk's z coordinate</param>
            public void MarkChunkAsModified(int x, int y, int z)
            {
                _modifiedChunksCoords.Add(new Vector3Int(x, y, z));
            }

            
            public void FrameUpdate()
            {
                mayRegenerateAndReplaceAllMeshes();

                // Debug.Log("rendering update not implemented yet");
                proceedToRegenerateAndReplaceAllModifiedMeshes();
            }

            void mayRegenerateAndReplaceAllMeshes()
            {
                // Debug.Log($"regenrate/replace: {_shouldRegenerateAndReplaceAllMeshes}");
                if (_shouldRegenerateAndReplaceAllMeshes)
                {
                    _shouldRegenerateAndReplaceAllMeshes = false;

                    foreach (KeyValuePair<(int, int, int), Chunk> kvp in _vc._chunkNet.chunks)
                    {
                        Chunk chunk = kvp.Value;
                        chunk.GenMesh();
                        UpdateChunkGridPosScaleVis(new Vector3(kvp.Key.Item1, kvp.Key.Item2, kvp.Key.Item3), chunk);
                    }

                    // we have just did this.
                    _modifiedChunksCoords.Clear();
                }
            }

            void proceedToRegenerateAndReplaceAllModifiedMeshes()
            {
                // Debug.Log($"modifiedMeshed {_modifiedChunksCoords.Count}");
                foreach (Vector3Int chunkCoord in _modifiedChunksCoords)
                {
                    Chunk chunk = _vc._chunkNet.Net[chunkCoord.x, chunkCoord.y, chunkCoord.z];
                    // chunk.GenMesh();
                    _vc._chunkMeshing.MeshChunk(chunk);
                    UpdateChunkGridPosScaleVis(new Vector3(chunkCoord.x, chunkCoord.y, chunkCoord.z), chunk);
                }
                _modifiedChunksCoords.Clear();
            }

            public void UpdateChunkGridPosScaleVis(Vector3 chunkGridPos, Chunk chunk)
            {
                // Debug.Log($"chunk at pos {chunkGridPos}");
                /*
                var signVec = ChunkNet.VoxelsView.makeSignVec((int)chunkGridPos.x, (int)chunkGridPos.y, (int)chunkGridPos.z);
                chunk.Trs.position = _vc._brushController.VoxelToWorldScaling(chunkGridPos) * Chunk.EdgeDimension - _vc._brushController.VoxelToWorldScaling(new Vector3(signVec.Item1, signVec.Item2, signVec.Item3)) ;
                */

                chunk.Trs.position = chunkGridPos * Chunk.EdgeDimension * _vc._voxelSize;

                chunk.Trs.localScale = _vc._brushController.VoxelToWorldScaling(Vector3.one);
                chunk.IsActive = true;
            }
        }
    }
}