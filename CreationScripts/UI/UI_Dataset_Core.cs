using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Dataset_Core : MonoBehaviour
{
    public GameObject Main_UI;

    // Two color classes defining the color used in each type of creation
    public Color Color_paint;
    public Color Color_voxel;

    // Creation Type, 0 for creating voxels and 1 for ink painting...
    public int Creation_Type;

    /////////////////////////////////////////////

    // Properties for 3d painting\
    public float painting_size;

    // Simple 3d painting or like splatton
    public float painting_type;

    /////////////////////////////////////////////

    public float s_value;

    // Five public properties for creating voxels
    public Voxel4.ToolAction Voxel_action;

    public float Voxel_offset;

    public Voxel4.ToolShape Voxel_shape;

    public float Voxel_voxel_size;

    public float Voxel_tool_size;

    void Start(){
        // Some initiation, should be the same in all scripts
        Color_paint = Color.green;
        Color_voxel = Color.green;
        
        Creation_Type = 0;
        s_value = 0.00f;

        Voxel_action = Voxel4.ToolAction.Write;
        Voxel_offset = 1.00f;
        Voxel_shape = Voxel4.ToolShape.UnitCube;
        Voxel_voxel_size = 1.00f;
        Voxel_tool_size = 1.00f;
    }

    void Update(){
        Debug.Log("Creation type:"+ Creation_Type);
        Debug.Log("Slider Value: "+ s_value);
    }
}
