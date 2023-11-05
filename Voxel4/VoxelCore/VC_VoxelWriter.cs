using UnityEngine;
using System;
using Voxel4.Internal;

namespace Voxel4
{
    public partial class VoxelCore
    {
        /// <summary>
        /// Provides ways to write voxels:
        /// - indiivually(with chunk Regen)
        /// - several voxels caching the modified chunks to only regen those
        /// - several voxels with no automatic chunk Regen(you'll have to
        /// manually ask for a regen)
        /// </summary>
        class VoxelWriter
        {
            VoxelCore _vc;

            public VoxelWriter(VoxelCore vc)
            {
                _vc = vc;
            }

            /// <summary>
            /// The simplest write function: it paints the voxel at the current
            /// brush's location, with the current color, shape and size
            /// </summary>
            public void PaintVoxel()
            {
                /*
                // _vc._brushController.Paint();
                // TODO
                throw new NotImplementedException("VCWriter PaintVoxel");
                */
                _vc._brushController.Paint();
            }

            /// <summary>
            /// Writes a voxel with the given parameters.
            /// Remembers the chunk that have been modified to
            /// only regen those later.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <param name="color"></param>
            public void WriteVoxel(int x, int y, int z, Color color)
            {
                
                _vc._chunkNet.Voxels[x, y, z] = new VoxelData(color);
                // Debug.Log($"writing voxel at {x} {y} {z}");
                var (chunkX, chunkY, chunkZ) = ChunkNet.VoxelsView.ChunkCoordFromVoxCoord(x, y, z);
                _vc._renderingController.MarkChunkAsModified(chunkX, chunkY, chunkZ);
                
                /*
                // TODO
                throw new NotImplementedException("VCWriter WriteVoxel");
                */
            }

            public void EraseVoxel(int x, int y, int z)
            {
                
                _vc._chunkNet.Voxels[x, y, z] = null;
                var (chunkX, chunkY, chunkZ) = ChunkNet.VoxelsView.ChunkCoordFromVoxCoord(x, y, z);
                _vc._renderingController.MarkChunkAsModified(chunkX, chunkY, chunkZ);
                
                /*
                // TODO
                throw new NotImplementedException("VCWriter EraseVoxel");
                */
            }
        }
    }
}