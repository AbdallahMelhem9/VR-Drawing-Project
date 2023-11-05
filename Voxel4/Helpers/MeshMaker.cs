using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Voxel4.Internal
{
    class MeshMaker
    {
        public List<Vector3> Vertices;
        public List<int> Triangles;
        public List<Color> Colors;

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
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
            Colors = new List<Color>();
        }

        public void AddFace(int faceIndex, Vector3 voxelCenterPos, Color voxelColor)
        {
            int o = Vertices.Count;
            for (int i = 0; i < 4; i++) Vertices.Add(voxelCenterPos + voxelVertices[VertexIndicesForFace[faceIndex, i]]);
            for (int i = 0; i < 4; i++) Colors.Add(voxelColor);
            Triangles.AddRange(new int[6] { o + 0, o + 1, o + 3, o + 1, o + 2, o + 3 });
        }

        public void AddVoxel(Vector3 voxelCenterPos, Color voxelColor)
        {
            int o = Vertices.Count;
            for (int i = 0; i < 8; i++) Vertices.Add(voxelCenterPos + voxelVertices[i]);
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
            Colors.Add(voxelColor / 10);
            Colors.Add(voxelColor);
            Colors.Add(voxelColor);
            Colors.Add(voxelColor * 2);
            Colors.Add(voxelColor / 10);
            Colors.Add(voxelColor / 20);
            Colors.Add(voxelColor / 2);
            Colors.Add(voxelColor / 10);
            //
            /* V5
            colors.Add(voxelColor / 2);
            colors.Add(voxelColor / 2);
            colors.Add(voxelColor * 2);
            colors.Add(voxelColor * 2);
            colors.Add(voxelColor / 2);
            colors.Add(voxelColor / 10);
            colors.Add(voxelColor * 2);
            colors.Add(voxelColor * 2);
            */
            /* V6
            colors.Add(voxelColor / 1);
            colors.Add(voxelColor / 2);
            colors.Add(voxelColor * 1);
            colors.Add(voxelColor * 2);
            colors.Add(voxelColor / 2);
            colors.Add(voxelColor / 10);
            colors.Add(voxelColor * 2);
            colors.Add(voxelColor * 1);
            */


            for (int i = 0; i < 6; i++)
            {
                Triangles.AddRange(new int[6]
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
            for (int i = 0; i < Vertices.Count; i++) sb.Append($"{Vertices[i]} ");
            sb.Append("\ntriangles:\n");
            for (int i = 0; i < Triangles.Count; i++) sb.Append($"{Triangles[i]} ");
            return sb.ToString();
        }
    }

}