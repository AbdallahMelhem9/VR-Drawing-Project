using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace Voxel4.Internal
{
    /// <summary>
    /// Data structure that controls a 3D Dictionnary of Chunks that grow
    /// whenever necessary, especially when inserting/retrieving individual Chunk
    /// or Voxel Data.
    /// Does NOT provide functions that operate on all chunks; that is the
    /// job of the VoxelCore.
    /// In particular, it does not:
    /// - place the chunks at the right location (it always places them at (0,0,0))
    /// - scale the chunks
    /// - tell them to regen their mesh
    /// 
    /// Since ChunkNet's job is to insert/retrieve individual Chunk/VoxelData,
    /// it actually works on 2 different coordinate systems:
    /// 1) the Net Grid coordinates
    /// 2) the Voxel Grid Coordinates
    /// (to work on the second it also needs to convert world voxel coordinates
    /// into local chunk coordinates, so this is technically a 3rd coordinate
    /// system, but it is an implementation detail)
    ///
    /// As such, depending on what you want, you are usually either going to
    /// use either
    /// - the Net attribute, to work on ChunkNet/Grid;
    /// - The Voxels attribute, to work on Voxels.
    /// 
    /// </summary>
    public class ChunkNet
    {
        public Dictionary<(int, int, int), Chunk> chunks { get; private set; }

        public NetView Net { get; private set; }
        public VoxelsView Voxels { get; private set; }

        public ChunkNet()
        {
            Net = new NetView(this);
            Voxels = new VoxelsView(this);

            chunks = new Dictionary<(int, int, int), Chunk>();
        }


        // ========== Net View ==========
        public class NetView
        {
            ChunkNet cn;

            public NetView(ChunkNet cn)
            {
                this.cn = cn;
            }

            /// <summary>
            /// Insert or retrieve a Chunk based on its coordinates, in the Net Grid.
            /// </summary>
            /// <param name="x">x coordinate in the chunk grid</param>
            /// <param name="y">y coordinate in the chunk grid</param>
            /// <param name="z">z coordinate in the chunk grid</param>
            /// <returns></returns>
            public Chunk this[int x, int y, int z]
            {
                get
                {
                    try
                    {
                        return cn.chunks[(x, y, z)];
                    }
                    catch (KeyNotFoundException)
                    {
                        Chunk ch = new Chunk(Vector3.zero);
                        cn.chunks[(x, y, z)] = ch;
                        return cn.chunks[(x, y, z)];
                    }
                }

                set => cn.chunks[(x, y, z)] = value;
            }
            /// <summary>
            /// Clear the data inside each chunk, without releasing GO
            /// </summary>
            public void ClearChunkContent()
            {
                foreach(KeyValuePair<(int, int, int), Chunk> kvp in cn.chunks)
                {
                    kvp.Value.ClearChunkData();
                }
            }
        }


        // ========== Voxels View ==========
        public class VoxelsView : IEnumerable
        {
            ChunkNet cn;

            public VoxelsView(ChunkNet cn)
            {
                this.cn = cn;
            }

            

            // ------ Enum Impl --------
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public VoxelViewEnum GetEnumerator()
            {
                return new VoxelViewEnum(cn);
            }
            // ------ End Enum Impl --------
            

            public VoxelData this[int x, int y, int z]
            {
                get
                {
                    var (chunkX, chunkY, chunkZ, locX, locY, locZ) =
                        chunkAndLocalCoordFromVoxCoord(x, y, z);

                    Chunk chunk = cn.Net[chunkX, chunkY, chunkZ];

                    return chunk[locX, locY, locZ];
                }
                set
                {
                    var (chunkX, chunkY, chunkZ, locX, locY, locZ) =
                        chunkAndLocalCoordFromVoxCoord(x, y, z);

                    Chunk chunk = cn.Net[chunkX, chunkY, chunkZ];


                    // Debug.Log($"x{x} y{y} z{z} | chunkX{chunkX}, chunkY{chunkY}, chunkZ{chunkZ} | locX{locX}, locY{locY}, locZ{locZ}");

                    chunk[locX, locY, locZ] = value;
                }
            }

            /// <summary>
            /// Make a sign vector associated to voxel coordinates.
            /// Sign vector is a tuple of integers where x component =
            /// * 0 if coordinate is positive
            /// * -1 if coordinate is striclty negative
            /// </summary>
            /// <param name="x"> x coordinate of the voxel </param>
            /// <param name="y"> y coordinate of the voxel </param>
            /// <param name="z"> z coordinate of the voxel </param>
            /// <returns>a sign vector</returns>
            public static (int, int, int) makeSignVec(int x, int y, int z)
            {
                int signVecX = x < 0 ? -1 : 0;
                int signVecY = y < 0 ? -1 : 0;
                int signVecZ = z < 0 ? -1 : 0;

                return (signVecX, signVecY, signVecZ);
            }

            /// <summary>
            /// Get the coordinates of the chunk that contains the voxel with the given coordiantes.
            /// </summary>
            /// <param name="x">x coordinate of the voxel</param>
            /// <param name="y">y coordinate of the voxel</param>
            /// <param name="z">z coordinate of the voxel</param>
            /// <returns>(x,y,z) of the Chunk</returns>
            public static (int, int, int) ChunkCoordFromVoxCoord(int x, int y, int z)
            {
                /*
                int signVecX = x < 0 ? -1 : 0;
                int signVecY = y < 0 ? -1 : 0;
                int signVecZ = z < 0 ? -1 : 0;
                */

                var (signVecX, signVecY, signVecZ) = makeSignVec(x, y, z);

                /*
                int chunkX = x / Chunk.Dimensions.x + signVecX;
                int chunkY = y / Chunk.Dimensions.y + signVecY;
                int chunkZ = z / Chunk.Dimensions.z + signVecZ;
                */
                int chunkX = (x - signVecX) / Chunk.Dimensions.x + signVecX;
                int chunkY = (y - signVecY) / Chunk.Dimensions.y + signVecY;
                int chunkZ = (z - signVecZ) / Chunk.Dimensions.z + signVecZ;

                return (chunkX, chunkY, chunkZ);
            }

            static (int, int, int, int, int, int) chunkAndLocalCoordFromVoxCoord(int x, int y, int z)
            {
                /*
                int signVecX = x < 0 ? -1 : 0;
                int signVecY = y < 0 ? -1 : 0;
                int signVecZ = z < 0 ? -1 : 0;
                */
                var (signVecX, signVecY, signVecZ) = makeSignVec(x, y, z);

                /*
                int chunkX = (x - signVecX) / Chunk.Dimensions.x + signVecX;
                int chunkY = (y - signVecY) / Chunk.Dimensions.y + signVecY;
                int chunkZ = (z - signVecZ) / Chunk.Dimensions.z + signVecZ;
                */
                var (chunkX, chunkY, chunkZ) = ChunkCoordFromVoxCoord(x, y, z);

                int locX = (-signVecX) * (Chunk.Dimensions.x - 1) + ((x - signVecX) % Chunk.Dimensions.x);
                int locY = (-signVecY) * (Chunk.Dimensions.y - 1) + ((y - signVecY) % Chunk.Dimensions.y);
                int locZ = (-signVecZ) * (Chunk.Dimensions.z - 1) + ((z - signVecZ) % Chunk.Dimensions.z);

                return (chunkX, chunkY, chunkZ, locX, locY, locZ);
            }

            
        }

        /// <summary>
        /// Returns an iterator over all voxels, including empty ones.
        /// (int, int, int, Color)
        /// integers are the absolute grid position of the voxels
        /// </summary>
        public class VoxelViewEnum : IEnumerator
        {
            ChunkNet _cn;

            Dictionary<(int, int, int), Chunk>.Enumerator _dictEnum;
            int voxX = 15;
            int voxY = 15;
            int voxZ = 15;

            public VoxelViewEnum(ChunkNet cn)
            {
                _cn = cn;
                _dictEnum = cn.chunks.GetEnumerator();
            }

            public (int, int, int, VoxelData) Current {
                get
                {
                    /*
                    try
                    {
                        // ...
                    } catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                    */
                    var kvp = _dictEnum.Current;
                    Vector3Int chunkCoord = new Vector3Int(kvp.Key.Item1, kvp.Key.Item2, kvp.Key.Item3);
                    Vector3Int relVoxCoord = new Vector3Int(voxX, voxY, voxZ);

                    Vector3Int absVoxCoord = absCoorFromRelNChunkCoord(relVoxCoord, chunkCoord);
                    VoxelData vd = kvp.Value[voxX, voxY, voxZ];
                    return (absVoxCoord.x, absVoxCoord.y, absVoxCoord.z, vd);
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                voxX++;
                if(voxX >= 16)
                {
                    voxX = 0;
                    voxY++;
                }
                if(voxY >= 16)
                {
                    voxY = 0;
                    voxZ++;
                }
                if(voxZ >= 16)
                {
                    voxZ = 0;
                    return _dictEnum.MoveNext();
                }
                return true;
            }

            public void Reset()
            {
                _dictEnum = _cn.chunks.GetEnumerator();
                voxX = 15;
                voxY = 15;
                voxZ = 15;
            }

            Vector3Int absCoorFromRelNChunkCoord(Vector3Int relCoord, Vector3Int chunkCoord)
            {
                // (-17,-17,-17) -> c(-2,-2,-2) v(15,15,15)
                // (-16,-16,-16) -> c(-1,-1,-1) v(0,0,0)
                // (-1,-1,-1) -> c(-1,-1,-1) v(15,15,15)
                // (0,0,0) -> c(0,0,0) v(0,0,0)
                // (1,1,1) -> c(0,0,0) v(1,1,1)
                // (15,15,15) -> c(0,0,0) v(15,15,15)
                // (16, 16, 16) -> c(1,1,1) v(0,0,0)
                // c*16 + v
                return Chunk.EdgeDimension * chunkCoord + relCoord;
            }
        }
    }
}

