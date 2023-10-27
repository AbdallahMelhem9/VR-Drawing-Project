using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class control_pose_vr : MonoBehaviour
{
    // private Transform controllerTransform;
    // private bool isControllerConnected;

    // private OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    // private OVRInput.Controller leftController = OVRInput.Controller.LTouch;

    // void Start()
    // {
    //     Debug.Log(this.transform.position+","+this.transform.localEulerAngles);
    //     // 获取Oculus手柄的Transform
    //     OVRInput.Controller activeController = OVRInput.GetActiveController();
    //     //controllerTransform = OVRInput.GetLocalControllerTransform(rightController);
    // }

    // void Update()
    // {
    //     // 检查手柄是否连接
    //     isControllerConnected = OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) ||
    //                           OVRInput.IsControllerConnected(OVRInput.Controller.RTouch);

    //     if (isControllerConnected)
    //     {
    //         // 获取手柄的位置和旋转
    //         Vector3 controllerPosition =  OVRInput.GetLocalControllerPosition(rightController);
    //         Quaternion controllerRotation =  OVRInput.GetLocalControllerRotation(rightController);

    //         // 设置物体的位置和旋转为手柄的位置和旋转
    //         this.transform.position = controllerPosition;
    //         this.transform.rotation = controllerRotation;
    //     }
    // }

    
    // public PrimaryButtonWatcher watcher;
    // public bool IsPressed = false; // used to display button state in the Unity Inspector window
    // public Vector3 rotationAngle = new Vector3(45, 45, 45);
    // public float rotationDuration = 0.25f; // seconds
    // private Quaternion offRotation;
    // private Quaternion onRotation;
    // private Coroutine rotator;

    // void Start(){
    //     watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
    //     offRotation = this.transform.rotation;
    //     onRotation = Quaternion.Euler(rotationAngle) * offRotation;
    // }

    // public void onPrimaryButtonEvent(bool pressed) {
    //     IsPressed = pressed;
    //     if (rotator != null)
    //         StopCoroutine(rotator);
    //     if (pressed)
    //         rotator = StartCoroutine(AnimateRotation(this.transform.rotation, onRotation));
    //     else
    //         rotator = StartCoroutine(AnimateRotation(this.transform.rotation, offRotation));
    // }

    // private IEnumerator AnimateRotation(Quaternion fromRotation, Quaternion toRotation)
    // {
    //     float t = 0;
    //     while (t < rotationDuration)
    //     {
    //         transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t / rotationDuration);
    //         t += Time.deltaTime;
    //         yield return null;
    //     }
    // }

}