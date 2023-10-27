using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_rotation_with_joystick : MonoBehaviour
{
    //public Transform rightController; // 右手VR控制器的Transform
    public float rotationSpeed = 2.0f; // 旋转速度

    public GameObject capsule;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Update()
    {
        // 获取右手VR控制器的摇杆输入
        float horizontalInput = Input.GetAxis("Oculus_GearVR_RThumbstickX");
        float verticalInput = Input.GetAxis("Oculus_GearVR_RThumbstickY");

        // 计算旋转角度
        rotationX += verticalInput * rotationSpeed;
        rotationY += horizontalInput * rotationSpeed;

        // 限制角度范围（可选）
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        // 应用旋转
        this.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        capsule.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        // 使摄像机跟随右手VR控制器的位置
        // if (rightController != null)
        // {
        //     transform.position = rightController.position;
        // }
    }
}





