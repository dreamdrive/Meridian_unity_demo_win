//関節を担う空のオブジェクトにアタッチする
//空のオブジェクトの書式は、[LRC]+[ID]+[_]+NAME]+[_]+[RPY][XYZ]　　例）L05_HipJointUpAxis_YY
//冒頭のLは左系統のサーボID、Rは右系統のサーボID、Cは右系統に属するが回転方向だけL系統に合わせたい場合（今回の例では中央に属する腰部）
using UnityEngine;
using System;

public class ID : MonoBehaviour
{
    private GameObject parent;
    public int servoId;
    private GameObject parammaster;

    public string lR;
    public string aXIS;
    private float[] angle;
    private ParamMaster script;

    public float x;
    public float y;
    public float z;

    void Start()
    {
        servoId = Convert.ToInt32((this.name.Substring(1, 2)));
        aXIS = this.name.Substring(this.name.Length - 1, 1);
        lR = this.name.Substring(0, 1);

    }

    void FixedUpdate()//パラムマスターの計算
    {
        if (lR == "L")
        {
            angle = ParamMaster.ServoAngles_L;
        }
        else
        {
            angle = ParamMaster.ServoAngles_R;
        }
        Vector3 rot = this.transform.localEulerAngles;

        if (aXIS == "Y")
        {
            rot.x = 0;
            rot.z = 0;
            if (lR == "R")
            {
                rot.y = angle[servoId];
            }
            else
            {
                rot.y = -angle[servoId];
            }
        }

        else if (aXIS == "Z")
        {
            rot.x = 0;
            rot.y = 0;

            if (lR == "R")
            {
                rot.z = -angle[servoId];
            }
            else
            {
                rot.z = angle[servoId];
            }
        }

        else
        {
            rot.x = -angle[servoId];
            rot.y = 0;
            rot.z = 0;
        }
        //this.transform.localEulerAngles = rot;
        this.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);


    }

    // Update is called once per frame
    void Update()
    {

    }
}