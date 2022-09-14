using UnityEngine;
using UnityEngine.UI;
using System;

public class TextOp : MonoBehaviour
{
    public GameObject value_display; // Textオブジェクト
    private GameObject parent;
    private int servoNum;
    private string lR;


    void Start()
    {
        parent = transform.parent.gameObject;
        servoNum = Convert.ToInt32(parent.name.Substring(1, 2));
        lR = parent.name.Substring(0, 1);
    }

    void Update()
    {
        Text display_text = value_display.GetComponent<Text>();

        if (lR == "L")
        {
            display_text.text = ParamMaster.ServoAngles_L_diff[servoNum].ToString("N2"); //小数点2位以下を非表示
        }
        else
        {
            display_text.text = ParamMaster.ServoAngles_R_diff[servoNum].ToString("N2"); //小数点2位以下を非表示
        }

    }
}