using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


class MeshMaker
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Color> colors;

    public static Vector3[] voxelVertices = new Vector3[8]
    {
        new Vector3(+0.5f, -0.5f, -0.5f),
        new Vector3(+0.5f, -0.5f, +0.5f),
        new Vector3(+0.5f, +0.5f, +0.5f),
        new Vector3(+0.5f, +0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, +0.5f),
        new Vector3(-0.5f, +0.5f, +0.5f),
        new Vector3(-0.5f, +0.5f, -0.5f),
    };

    public static int[,] VertexIndicesForFace = new int[6, 4]
    {
        { 3, 2, 1, 0 }, // xPos
        { 6, 7, 4, 5 }, // xNeg
        { 6, 2, 3, 7 }, // yPos
        { 4, 0, 1, 5 }, // yNeg
        { 2, 6, 5, 1 }, // zPos
        { 7, 3, 0, 4 }, // zNeg
    };

    public MeshMaker()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    public void addFace(int faceIndex, Vector3 voxelCenterPos, Color voxelColor)
    {
        int o = vertices.Count;
        for (int i = 0; i < 4; i++) vertices.Add(voxelCenterPos + voxelVertices[VertexIndicesForFace[faceIndex, i]]);
        for (int i = 0; i < 4; i++) colors.Add(voxelColor);
        triangles.AddRange(new int[6] { o + 0, o + 1, o + 3, o + 1, o + 2, o + 3 });
    }

    public void addVoxel(Vector3 voxelCenterPos, Color voxelColor)
    {
        int o = vertices.Count;
        for (int i = 0; i < 8; i++) vertices.Add(voxelCenterPos + voxelVertices[i]);
        /* V1
        for (int i = 0; i < 8; i++) colors.Add(voxelColor);
        */
        /* V2
        for (int i = 0; i < 2; i++) colors.Add(voxelColor);
        for (int i = 0; i < 2; i++) colors.Add(voxelColor / 10);
        for (int i = 0; i < 2; i++) colors.Add(voxelColor);
        for (int i = 0; i < 2; i++) colors.Add(voxelColor / 10);
        */
        /* V3
        for (int i = 0; i < 2; i++) colors.Add(voxelColor / 10);
        for (int i = 0; i < 2; i++) colors.Add(voxelColor);
        for (int i = 0; i < 2; i++) colors.Add(voxelColor / 10);
        for (int i = 0; i < 2; i++) colors.Add(voxelColor);
        */
        // V4
        colors.Add(voxelColor / 10);
        colors.Add(voxelColor);
        colors.Add(voxelColor);
        colors.Add(voxelColor * 2);
        colors.Add(voxelColor / 10);
        colors.Add(voxelColor / 20);
        colors.Add(voxelColor / 2);
        colors.Add(voxelColor / 10);
        //


        for (int i = 0; i < 6; i++)
        {
            triangles.AddRange(new int[6]
            {
                o + VertexIndicesForFace[i, 0],
                o + VertexIndicesForFace[i, 1],
                o + VertexIndicesForFace[i, 3],
                o + VertexIndicesForFace[i, 1],
                o + VertexIndicesForFace[i, 2],
                o + VertexIndicesForFace[i, 3]
            });
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("vertices:\n");
        for (int i = 0; i < vertices.Count; i++) sb.Append($"{vertices[i]} ");
        sb.Append("\ntriangles:\n");
        for (int i = 0; i < triangles.Count; i++) sb.Append($"{triangles[i]} ");
        return sb.ToString();
    }
}
