using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMainCamera : MonoBehaviour
{
    public Transform camera_transform;
    public Transform camera_ui_transform;
    private Vector3 rotate_direction;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rotate_direction = camera_ui_transform.position - camera_transform.position;
        this.transform.position = camera_ui_transform.position;
        //this.transform.rotation = Quaternion.LookRotation(rotate_direction);
    }
}
