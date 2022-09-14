using UnityEngine;
using UnityEngine.UI;
using System;

// サーボ操作GUIパネルのスライダーに入力された数値を、ParamMasterに送る
public class SliderOp : MonoBehaviour
{
    private GameObject parent;
    private int servoNum;

    private Slider slider;
    private string lR;

    void Start()
    {
        //parammaster = GameObject.Find("ParamMaster");
        slider = GetComponent<Slider>();
        parent = transform.parent.gameObject;
        servoNum = Convert.ToInt32(parent.name.Substring(1, 2));
        lR = parent.name.Substring(0, 1);
    }

    void FixedUpdate()
    {
        //script = parammaster.GetComponent<ParamMaster>();
        if (lR == "L")
        {
            if (ParamMaster.ServoCommand_L_s[servoNum] == 0)//サーボコマンドが0ならばパラムマスターの値を反映
            {
                slider.value = ParamMaster.ServoAngles_L_diff[servoNum];
                //ParamMaster.ServoAngles_L_UI[servoNum] = slider.value;
            }
            else
            {
                //slider.value = ParamMaster.ServoAngles_L_diff[servoNum];
            }
        }
        else
        {
            if (ParamMaster.ServoCommand_R_s[servoNum] == 0)//サーボコマンドが0ならばパラムマスターの値を反映
            {
                slider.value = ParamMaster.ServoAngles_R_diff[servoNum];
            }
            else
            {
                //slider.value = ParamMaster.ServoAngles_L_diff[servoNum];
            }
        }
    }

    void Update()
    {

    }

    public void SlidernewValue(float newValue)
    {
        if (lR == "L")
        {
            ParamMaster.ServoAngles_L_UI[servoNum] = newValue;
            //slider.value = script.ServoAngles_L[servoNum];
        }
        else
        {
            ParamMaster.ServoAngles_R_UI[servoNum] = newValue;
            //slider.value = script.ServoAngles_R[servoNum];

        }
    }
}