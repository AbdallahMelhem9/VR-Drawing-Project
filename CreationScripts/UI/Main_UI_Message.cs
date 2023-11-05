using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_UI_Message : MonoBehaviour
{
    public GameObject creation_type_list;
    public GameObject voxel_brush_offset_slider;
    public TextMeshProUGUI voxel_brush_offset_slider_display;
    public GameObject voxel_shape_list;
    public GameObject voxel_voxel_size_slider;
    public TextMeshProUGUI voxel_voxel_size_slider_display;
    public GameObject voxel_tool_size_slider;
    public TextMeshProUGUI voxel_tool_size_slider_display;
    public UI_Dataset_Core core;



    void Start(){
    }

    void Update(){
        
    }

    public void Choose_Creation_Type() {
        core.Creation_Type = creation_type_list.GetComponent<TMP_Dropdown>().value;
    }

    public void voxel_action_change(){
        if (core.Voxel_action == Voxel4.ToolAction.Write){
            core.Voxel_action = Voxel4.ToolAction.Erase;
        }else{
            core.Voxel_action = Voxel4.ToolAction.Write;
        }
    }

    public void voxel_brush_offset_change(){
        core.Voxel_offset = voxel_brush_offset_slider.GetComponent<Slider>().value;
        voxel_brush_offset_slider_display.text = string.Format("{0:F2}",voxel_brush_offset_slider.GetComponent<Slider>().value);
    }

    public void voxel_shape_change(){
        switch (voxel_shape_list.GetComponent<TMP_Dropdown>().value){
            case 0:
                core.Voxel_shape = Voxel4.ToolShape.UnitCube;
                break;
            case 1:
                core.Voxel_shape = Voxel4.ToolShape.Cube;
                break;
            case 2:
                core.Voxel_shape = Voxel4.ToolShape.RotatedCube;
                break;
            case 3:
                core.Voxel_shape = Voxel4.ToolShape.Line;
                break;
            case 4:
                core.Voxel_shape = Voxel4.ToolShape.Sphere;
                break;
            case 5:
                core.Voxel_shape = Voxel4.ToolShape.RotatedSphere;
                break;
        }
    }

    public void voxel_voxel_size_change(){
        core.Voxel_voxel_size = voxel_voxel_size_slider.GetComponent<Slider>().value;
        voxel_voxel_size_slider_display.text = string.Format("{0:F2}",voxel_voxel_size_slider.GetComponent<Slider>().value);
    }

    public void voxel_tool_size_change(){
        core.Voxel_tool_size = voxel_tool_size_slider.GetComponent<Slider>().value;
        voxel_tool_size_slider_display.text = string.Format("{0:F2}",voxel_tool_size_slider.GetComponent<Slider>().value);
    }
}
