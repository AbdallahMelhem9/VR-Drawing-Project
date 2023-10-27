using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCore : MonoBehaviour
{
    public GameObject chunkGO;
    ChunkNet cn;

    // Start is called before the first frame update
    void Start()
    {
        Chunk.ChunkTemplate = chunkGO;
        cn = new ChunkNet(new Vector3Int(1, 1, 1));
    }

    public void WriteVoxel(int x, int y, int z, Color color)
    {
        cn.insertVoxel(new VoxelData(color), x, y, z);
        // TODO
        // -- remove this, and write a better ChunkNet API --
        int chunkX = x / Chunk.Dimensions.x;
        int chunkY = y / Chunk.Dimensions.y;
        int chunkZ = z / Chunk.Dimensions.z;
        // ----
        Chunk chunk = cn.RetrieveChunk(chunkX, chunkY, chunkZ);
        chunk.genMesh();
        chunk.Rendering = true;
    }

    public void WriteVoxelNoGen(int x, int y, int z, Color color)
    {
        cn.insertVoxel(new VoxelData(color), x, y, z);
    }

    public void RegenChunks()
    {
        Common.ActOnMatrixIterate(cn.Dimensions.x, cn.Dimensions.y, cn.Dimensions.z, (x, y, z) =>
        {
            Chunk c = cn.RetrieveChunk(x, y, z);
            c.genMesh();
            c.Rendering = true;
            // Debug.Log($"chunk {x} {y} {y} reloaded");
        });
    }
}
