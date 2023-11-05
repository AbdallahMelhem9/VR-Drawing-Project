using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.InputSystem;


public class Select_Color : MonoBehaviour
{
    public UI_Dataset_Core core;
    public GameObject controller;
    public PrimaryButtonWatcher watcher;
    public GameObject view_hit;
    public GameObject Palette;
    public MeshRenderer meshRenderer;
    public float time = 0.00f;
    public bool fix = true;
    void Start(){
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
        meshRenderer.material.color = Color.green;

    }

    void Update(){
        Debug.Log(Keyboard.current.hKey.isPressed);
    }

    public void onPrimaryButtonEvent(bool pressed) {
        RaycastHit hit;

        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, Mathf.Infinity) && hit.collider == Palette.GetComponent<Collider>())
        {
            var hit_point = hit.point;
            view_hit.transform.position = new Vector3(hit_point.x,hit_point.y,hit_point.z);
        }

        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, Mathf.Infinity) && hit.collider == Palette.GetComponent<Collider>())
        {
            Vector2 uv = hit.textureCoord;
            SpriteRenderer colorPaletteRenderer = Palette.GetComponent<SpriteRenderer>(); 
            Texture2D texture = colorPaletteRenderer.sprite.texture;
            Color selectedColor = texture.GetPixelBilinear(1-uv.x, 1-uv.y);
            meshRenderer.material.color = selectedColor;
            if (core.Creation_Type==1){
                core.Color_paint = selectedColor;
            }else{
                core.Color_voxel = selectedColor;
            }
            Debug.Log($"Selected Color: {selectedColor}, x:{uv.x}, y:{uv.y}");
        }
    }
}
