using UnityEngine;
using UnityEngine.XR;

using System;
using Voxel4.Internal;

namespace Voxel4
{
    public partial class VoxelCore
    {
        /// <summary>
        /// Meshing algorithm + culling
        /// Does not detect when chunks should be meshed again.
        /// Typically this is done either by the VoxelWriter or the RenderingController
        /// </summary>
        class ChunkMeshing
        {
            public static float voxelSize = 1.0f;
            VoxelCore _vc;

            public ChunkMeshing(VoxelCore vc)
            {
                _vc = vc;
            }

            public void MeshChunk(Chunk chunk)
            {
                // TODO: rewrite meshing in this class instead
                chunk.GenMesh();
            }
        }
    }
}