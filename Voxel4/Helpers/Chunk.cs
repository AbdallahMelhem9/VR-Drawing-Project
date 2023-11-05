using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel4.Internal
{
    /// <summary>
    /// The Chunk is the Entity that manages a 3D array of voxels.
    /// It:
    /// - provides a way to insert and retrieve voxels in chunk coordinates
    /// - provides a method to rebuild the Chunk's mesh
    /// - manages the lifetime of its GameObject.
    ///
    /// The Chunk template should be defined once and for all. Modifications
    /// to it don't affects existing chunks.
    ///
    /// By default, the chunk's associated GO is inactive, so you can
    /// place it, scale it before showing it.
    /// </summary>
    public class Chunk
    {
        public static int EdgeDimension = 16;
        public static GameObject CommonChunkTemplate;
        public static Vector3Int Dimensions = new Vector3Int(EdgeDimension, EdgeDimension, EdgeDimension);
        public VoxelData[,,] Voxels { get; private set; }

        GameObject chunkGO;

        public bool IsActive
        {
            get => chunkGO.activeInHierarchy;
            set => chunkGO.SetActive(value);
        }

        public Transform Trs
        {
            get => chunkGO.GetComponent<Transform>();
            set
            {
                Transform t = chunkGO.GetComponent<Transform>();
                t.position = value.position;
                t.rotation = value.rotation;
                t.localScale = value.localScale;
            }
        }

        public Chunk(Vector3 pos)
        {
            Voxels = new VoxelData[Dimensions.x, Dimensions.y, Dimensions.z];

            chunkGO = GameObject.Instantiate(CommonChunkTemplate, pos, Quaternion.identity);
            chunkGO.SetActive(false);
        }

        ~Chunk()
        {
            Release();
        }

        public void Release()
        {
            if(chunkGO != null)
            {
                GameObject.Destroy(chunkGO);
                chunkGO = null;
            }
        }

        /// <summary>
        /// Clear the data in the chunk, without releasing GO
        /// </summary>
        public void ClearChunkData()
        {
            Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
            {
                Voxels[x, y, z] = null;
            });
        }

        /// <summary>
        /// Get voxel data in chunk space.
        ///
        /// WARNING: invalid values are not prevented to reach the underlying
        /// array, so beware of crashes.
        /// </summary>
        /// <param name="x">x coordinate in chunk space</param>
        /// <param name="y">y coordinate in chunk space</param>
        /// <param name="z">z coordinate in chunk space</param>
        /// <returns></returns>
        public VoxelData this[int x, int y, int z]
        {
            get => Voxels[x, y, z];
            set => Voxels[x, y, z] = value;
        }

        /// <summary>
        /// Sets the mes from the given parameters. WARNING:
        /// setting the mesh does not automatically set the game object
        /// in an Active State.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="normals"></param>
        /// <param name="colors"></param>
        /// <param name="triangles"></param>
        public void SetMesh(
            List<Vector3> vertices,
            List<Vector3> normals,
            List<Color> colors,
            List<int> triangles)
        {
            Mesh mesh = new Mesh()
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                colors = colors.ToArray(),
                normals = normals.ToArray()
            };
            mesh.Optimize();
            chunkGO.GetComponent<MeshFilter>().mesh = mesh;
        }

        public void SetMeshNoNormals(
            List<Vector3> vertices,
            List<Color> colors,
            List<int> triangles)
        {
            Mesh mesh = new Mesh()
            {
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                colors = colors.ToArray(),
            };
            mesh.Optimize();
            chunkGO.GetComponent<MeshFilter>().mesh = mesh;
        }

        // ------------------------ Uncommented -----------------------
        // ----- This will have to be implemented in ChunkMeshing -----
        // ------------------------------------------------------------

        public void GenMesh()
        {
            MeshMaker mm = new MeshMaker();

            Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
            {
                VoxelData voxel = Voxels[x, y, z];
                if (voxel != null)
                {
                    // mm.AddVoxel(new Vector3(x, y, z), voxel.Color);
                    if (isVoxelNotSurrounded(x, y, z))
                    {
                        mm.AddVoxel(new Vector3(x, y, z), voxel.Color);
                    }
                }
            });
            Mesh mesh = new Mesh()
            {
                vertices = mm.Vertices.ToArray(),
                triangles = mm.Triangles.ToArray(),
                colors = mm.Colors.ToArray(),
            };
            mesh.RecalculateNormals();
            chunkGO.GetComponent<MeshFilter>().mesh = mesh;
        }

        // does not check whether the voxel we are talking about actually exists.
        bool isVoxelNotSurrounded(int x, int y, int z)
        {
            return !(isVoxelOccupied(x - 1, y, z)
                && isVoxelOccupied(x + 1, y, z)
                && isVoxelOccupied(x, y - 1, z)
                && isVoxelOccupied(x, y + 1, z)
                && isVoxelOccupied(x, y, z - 1)
                && isVoxelOccupied(x, y, z + 1));
        }

        // Does accept negative values.
        //
        // For now, not cross chunk computations are done.
        bool isVoxelOccupied(int x, int y, int z)
        {
            return !(x < 0 || y < 0 || z < 0 || x >= Dimensions.x || y >= Dimensions.y || z >= Dimensions.z) && Voxels[x, y, z] != null;
        }
        
    }
}

