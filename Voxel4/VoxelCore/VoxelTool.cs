using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxel4
{
    [System.Serializable]
    public class VoxelTool
    {
        [SerializeField]
        float _toolSize = 1.0f;
        public float ToolSize
        {
            get => _toolSize;
            set => _toolSize = Mathf.Max(1, value);
        }

        public ToolAction Action = ToolAction.Write;


        [SerializeField]
        ToolShape _shape = ToolShape.UnitCube;
        public ToolShape Shape
        {
            get => _shape;
            set
            {
                // From small to big
                if(_shape == ToolShape.UnitCube || _shape == ToolShape.Line)
                {
                    if(value == ToolShape.Cube || value == ToolShape.RotatedCube)
                    {
                        // Debug.Log("from small to big");
                        BrushOffset += (ToolSize + 1) / 2;
                    } else if (value == ToolShape.Sphere || value == ToolShape.RotatedSphere)
                    {
                        Debug.Log("from small to very big");
                        BrushOffset += ToolSize + 1;
                    }
                    // From Bif to small
                } else if(value == ToolShape.UnitCube || value == ToolShape.Line)
                {
                    if(_shape == ToolShape.Cube || _shape == ToolShape.RotatedCube)
                    {
                        // Debug.Log("from big to small");
                        BrushOffset -= ToolSize / 2;
                    } else if (_shape == ToolShape.Sphere || _shape == ToolShape.RotatedSphere)
                    {
                        // Debug.Log("from big to very small");
                        BrushOffset -= ToolSize + 1;
                    }
                }

                _shape = value;
            }
        }

        public Color Color = Color.green;

        /*
        [SerializeField]
        public float _brushOffset = 1.0f;
        public float BrushOffset
        {
            get => _brushOffset;
            set
            {
                if(value == 1)
                {
                    // Debug.Log(System.Environment.StackTrace);
                }
                Debug.Log($"old bo:{_brushOffset} | new:{value}");
                _brushOffset = Mathf.Max(0, value);
            }
        }
        */
        [SerializeField]
        public float _brushOffset = 1.0f;
        public float BrushOffset
        {
            get => _brushOffset;
            set => _brushOffset = Mathf.RoundToInt(Mathf.Max(0, value));
        }
    }

    [System.Serializable]
    public enum ToolAction
    {
        Write,
        Erase
    }

    [System.Serializable]
    public enum ToolShape
    {
        UnitCube,
        Cube,
        RotatedCube,
        Line,
        Sphere,
        RotatedSphere,
    }
}