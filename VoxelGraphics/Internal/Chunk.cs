using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public static int ChunkCreationCount = 0;
    public static GameObject ChunkTemplate;
    public static Vector3Int Dimensions = new Vector3Int(16, 16, 16);

    VoxelData[,,] voxels;
    GameObject chunkGO;

    public bool Rendering
    {
        get => chunkGO.activeInHierarchy;
        set => chunkGO.SetActive(value);
    }

    public Transform Trs
    {
        get => chunkGO.GetComponent<Transform>();
        set {
            Transform t = chunkGO.GetComponent<Transform>();
            t = value;
        }
    }

    public Chunk(Vector3 pos)
    {
        voxels = new VoxelData[Dimensions.x, Dimensions.y, Dimensions.z];
        chunkGO = GameObject.Instantiate(ChunkTemplate, pos, Quaternion.identity);
        chunkGO.SetActive(false);
        ChunkCreationCount++;
    }

    public void release()
    {
        GameObject.Destroy(chunkGO);
    }

    public VoxelData this[int x, int y, int z]
    {
        get => voxels[x, y, z];
        set => voxels[x, y, z] = value;
    }

    public void genMesh()
    {
        MeshMaker mm = new MeshMaker();

        Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
        {
            VoxelData voxel = voxels[x, y, z];
            if (voxel != null)
            {
                // mm.addVoxel(new Vector3(x, y, z), voxel.Color);
                if (isVoxelNotSurrounded(x, y, z))
                {
                    mm.addVoxel(new Vector3(x, y, z), voxel.Color);
                }
            }
        });

        chunkGO.GetComponent<MeshFilter>().mesh = new Mesh()
        {
            vertices = mm.vertices.ToArray(),
            triangles = mm.triangles.ToArray(),
            colors = mm.colors.ToArray(),
        };
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
        return !(x < 0 || y < 0 || z < 0 || x >= Dimensions.x || y >= Dimensions.y || z >= Dimensions.z) && voxels[x, y, z] != null;
    }
}
