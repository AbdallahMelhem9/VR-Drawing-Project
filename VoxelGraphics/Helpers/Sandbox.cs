using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sandbox : MonoBehaviour
{
    public GameObject chunkGO;
    int logCount = 5;

    static Color[] availableColors = new Color[]
    {
        Color.white,
        Color.grey,
        Color.black,
        Color.red,
        Color.green,
        Color.blue,
        Color.cyan,
        Color.yellow,
        Color.magenta
    };

    void Start()
    {
        test3();
    }

    void test3()
    {
        Chunk.ChunkTemplate = chunkGO;
        ChunkNet cn = new ChunkNet(new Vector3Int(1, 1, 1));
        Common.ActOnMatrixIterate(32, 32, 32, (x, y, z) =>
        {
            VoxelData vd = new VoxelData(availableColors[(x + y + z) % availableColors.Length]);
            cn.insertVoxel(vd, x, y, z);
        });
        Common.ActOnMatrixIterate(cn.Dimensions.x, cn.Dimensions.y, cn.Dimensions.z, (x, y, z) =>
        {
            Chunk c = cn.RetrieveChunk(x, y, z);
            c.genMesh();
            c.Rendering = true;
        });
        Debug.Log(Chunk.ChunkCreationCount);
    }

    void test2()
    {
        Chunk.ChunkTemplate = chunkGO;
        Chunk c = new Chunk(Vector3.zero);
        Common.ActOnMatrixIterate(Chunk.Dimensions.x, Chunk.Dimensions.y, Chunk.Dimensions.z, (x, y, z) =>
        {
            c[x, y, z] = new VoxelData(availableColors[(x + y + z) % availableColors.Length]);
        });
        c.genMesh();
        c.Rendering = true;
    }

    // Start is called before the first frame update
    void test1()
    {
        var mm = new MeshMaker();

        // mm.addFace(0, Vector3.zero, Color.red);
        //drawCube(mm, Vector3.zero, Color.red);
        /*
        actOnMatrixIterate(16, 16, 16, (x, y, z) =>
        {
            Color c = availableColors[(x + y + z) % availableColors.Length];
            drawCube(mm, new Vector3(x, y, z), c);
        });*/
        Common.ActOnMatrixIterate(16, 16, 16, (x, y, z) =>
        {
            Color c = availableColors[(x + y + z) % availableColors.Length];
            drawCube(mm, new Vector3(x, y, z), c);
        });

        Mesh m = new Mesh();
        m.vertices = mm.vertices.ToArray();
        m.triangles = mm.triangles.ToArray();
        m.colors = mm.colors.ToArray();
        chunkGO.GetComponent<MeshFilter>().mesh = m;
    }

    void drawCube(MeshMaker mm, Vector3 pos, Color color)
    {
        // for (int i = 0; i < 6; i++) mm.addFace(i, pos, color);
        mm.addVoxel(pos, color);
        if(logCount >= 0)
        {
            Debug.Log(mm);
            logCount--;
        }
    }

    /*
    void actOnMatrixIterate(int xDim, int yDim, int zDim, Action<int, int, int> a)
    {
        for(int z = 0; z < zDim; z++)
        {
            for(int y = 0; y < yDim; y++)
            {
                for(int x = 0; x < xDim; x++)
                {
                    a(x, y, z);
                }
            }
        }
    }*/
}
