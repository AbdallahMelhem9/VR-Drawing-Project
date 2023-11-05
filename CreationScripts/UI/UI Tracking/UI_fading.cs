using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_fading : MonoBehaviour
{
    public GameObject Main_UI_canvas;
    private bool UI_active = false;

    public SecondaryButtonWatcher watcher;

    void Start(){
        UI_active = false;
        watcher.secondaryButtonPress.AddListener(onSecondaryButtonEvent);
    }

    public void Click_Toggle_Panel(){
        UI_active = !UI_active;
        Main_UI_canvas.SetActive(UI_active);
    }

    public void onSecondaryButtonEvent(bool pressed){
        UI_active = !UI_active;
        Main_UI_canvas.SetActive(UI_active);
    }

    void Update(){

    }

}
