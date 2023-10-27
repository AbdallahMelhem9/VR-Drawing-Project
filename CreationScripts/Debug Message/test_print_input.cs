using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_print_input : MonoBehaviour
{
    // The Oculus Touch controllers
    public GameObject rightController;
    public GameObject leftController;

    private float wait_time = 0.3f;
    private float timer = 0.0f;


    private Vector3 rightControllerPosition;
    private Vector3 leftControllerPosition;
    private Vector3 LastRightPos;
    private Vector3 LastLeftPos;

    public Transform rightControllerTF;
    public LayerMask raycastLayer; // Hit layer of the ray

    void Start(){
        // Some init
        LastRightPos = new Vector3(0.000f,0.000f,0.000f);
        LastLeftPos = new Vector3(0.000f,0.000f,0.000f);

    }
    

    void Update()
    {
        // Get the local position of the controller
        rightControllerPosition = rightController.transform.position;
        leftControllerPosition = leftController.transform.position;
        Vector3 Loffset = leftControllerPosition - LastLeftPos;
        float Ldistance = Loffset.magnitude;
        Vector3 Roffset = rightControllerPosition - LastRightPos;
        float Rdistance = Roffset.magnitude;

        timer+=Time.deltaTime;
        if (timer > wait_time){
            timer-=wait_time;
            // Improve the output decimal places to 3 (ori 1)
            string rightControllerPosStr = string.Format("({0:F3}, {1:F3}, {2:F3})", rightControllerPosition.x, rightControllerPosition.y, rightControllerPosition.z);
            string leftControllerPosStr = string.Format("({0:F3}, {1:F3}, {2:F3})", leftControllerPosition.x, leftControllerPosition.y, leftControllerPosition.z);
            //Print the positions and speseds to the console
            Debug.Log("Right Controller Position: " + rightControllerPosStr);
            Debug.Log("Left Controller Position: " + leftControllerPosStr);

            string LoffsetStr = string.Format("({0:F3}, {1:F3}, {2:F3})", Loffset.x, Loffset.y, Loffset.z);
            string RoffsetStr = string.Format("({0:F3}, {1:F3}, {2:F3})", Roffset.x, Roffset.y, Roffset.z);

            Debug.Log("Left Controller speed per frame: " + Ldistance + " Offset: " + LoffsetStr);
            Debug.Log("Right Controller speed per frame: " + Rdistance + " Offset: " + RoffsetStr);

            
        }
        LastRightPos = rightControllerPosition;
        LastLeftPos = leftControllerPosition;
        // Vector3 controllerPosition = rightControllerTF.position;
        // Vector3 controllerForward = rightControllerTF.forward;

        // Ray ray = new Ray(controllerPosition, controllerForward);
        // RaycastHit hitInfo;

        // if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, raycastLayer))
        // {
        //     Vector3 hitPoint = hitInfo.point;
        //     Debug.Log("The ray hits the point: " + hitPoint);

        // }
    }
}