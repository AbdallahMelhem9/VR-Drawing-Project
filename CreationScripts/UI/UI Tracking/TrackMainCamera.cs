using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMainCamera : MonoBehaviour
{
    public Transform camera_transform;
    public Transform camera_ui_transform;
    private Vector3 rotate_direction;
    public bool if_rotate;
    private Vector3 offset;

    void Start(){
        offset = new Vector3(0,1,0);
    }
    // Update is called once per frame
    void Update()
    {
        rotate_direction = camera_ui_transform.position - camera_transform.position-offset;
        this.transform.position = camera_ui_transform.position;
        if (if_rotate){
            this.transform.rotation = Quaternion.LookRotation(rotate_direction);
        }
    }
}
