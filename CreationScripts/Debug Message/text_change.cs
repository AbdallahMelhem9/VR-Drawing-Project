using UnityEngine;
using TMPro;

public class text_change : MonoBehaviour
{
    public TextMeshProUGUI textMeshProText; // 引用TextMeshPro组件
    public string information; // 实时信息的变量
    // The Oculus Touch controllers
    public GameObject rightController;
    public GameObject leftController;

    private float wait_time = 0.3f;
    private float timer = 0.0f;


    private Vector3 rightControllerPosition;
    private Vector3 leftControllerPosition;
    private Vector3 LastRightPos;
    private Vector3 LastLeftPos;

    void Start(){
        // Some init
        LastRightPos = new Vector3(0.000f,0.000f,0.000f);
        LastLeftPos = new Vector3(0.000f,0.000f,0.000f);
    }

    // 在Update中更新UI文本
    void Update()
    {
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

            rightControllerPosStr = "R Controller Pos: " + rightControllerPosStr+"\n";
            leftControllerPosStr = "L Controller Pos: " + leftControllerPosStr+"\n";

            string LoffsetStr = string.Format("({0:F3}, {1:F3}, {2:F3})", Loffset.x, Loffset.y, Loffset.z);
            string RoffsetStr = string.Format("({0:F3}, {1:F3}, {2:F3})", Roffset.x, Roffset.y, Roffset.z);

            LoffsetStr = "L Controller speed per frame: " + string.Format("{0:F3}",Ldistance) + "\n";
            RoffsetStr = "R Controller speed per frame: " + string.Format("{0:F3}",Rdistance) + "\n";
            information = rightControllerPosStr+leftControllerPosStr+RoffsetStr+LoffsetStr;
            
        }
        LastRightPos = rightControllerPosition;
        LastLeftPos = leftControllerPosition;
        
        this.textMeshProText.text = information;
    }
}
