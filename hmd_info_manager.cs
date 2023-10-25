using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class hmd_info_manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Is device active "+XRSettings.isDeviceActive);
        Debug.Log("Device name is :"+XRSettings.loadedDeviceName);
        if(!XRSettings.isDeviceActive){
            Debug.Log("Headset connected");
        }else if(XRSettings.loadedDeviceName=="MockHMD"){
            Debug.Log("Using MockHMD instead");
        }else{
            Debug.Log("We use another headset"+XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
