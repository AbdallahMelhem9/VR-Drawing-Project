using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel4.Internal
{
    public static class Common
    {
        public static void ActOnMatrixIterate(int xDim, int yDim, int zDim, Action<int, int, int> a)
        {
            for (int z = 0; z < zDim; z++)
            {
                for (int y = 0; y < yDim; y++)
                {
                    for (int x = 0; x < xDim; x++)
                    {
                        a(x, y, z);
                    }
                }
            }
        }
    }
}


