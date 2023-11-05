using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creation_Sub_UI : MonoBehaviour
{
    public GameObject Sub_UI_canvas;

    public void Click_Toggle_Panel(){
        Sub_UI_canvas.SetActive(!Sub_UI_canvas.activeSelf);
    }

}
