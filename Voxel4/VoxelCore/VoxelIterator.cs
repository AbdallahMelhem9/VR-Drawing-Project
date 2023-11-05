using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Voxel4.Internal;

namespace Voxel4.Internal
{
    public class VoxelIterator
    {
        private ChunkNet.VoxelViewEnum _enum;

        public VoxelIterator(ChunkNet chunkNet)
        {
            _enum = chunkNet.Voxels.GetEnumerator();
        }

        public bool MoveNext()
        {
            while(_enum.MoveNext())
            {
                if(_enum.Current.Item4 != null)
                {
                    return true;
                }
            }
            return false;
        }

        public (Vector3Int, Color) Current()
        {
            var (x, y, z, data) = _enum.Current;
            return (new Vector3Int(x, y, z), data.Color);
        }

        public void Reset()
        {
            _enum.Reset();
        }
    }
}


