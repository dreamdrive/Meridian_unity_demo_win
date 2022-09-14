
using UnityEngine;
using UnityEngine.UI;
using System; //convertを使うために必要

// Canvas-Servos-R**-InputFeildにアタッチするスクリプト
public class InputFieldOp : MonoBehaviour
{
    GameObject parammaster;
    InputField inputField;
    Slider slider;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private int servoNum;

    [SerializeField]
    private string lR;

    void Start()
    {
        parent = transform.parent.gameObject;
        inputField = this.GetComponent<InputField>();
        parammaster = GameObject.Find("ParamMaster");
        slider = parent.transform.Find("Slider").GetComponent<Slider>();

        servoNum = Convert.ToInt32(parent.name.Substring(1, 2));
        lR = parent.name.Substring(0, 1);

    }

    public void IfPushEnter(string _)
    {
        //エンターが押されたら調べる（誤動作防止）
        Debug.Log(inputField);
        if (Input.GetKey(KeyCode.Return))
        {
            //InputFieldからテキスト情報を取得する
            string value = inputField.text;

            //入力されたものが数字かどうか判断する
            bool result = float.TryParse(value, out float _);
            if (result != false)
            {
                //ParamMaster script = parammaster.GetComponent<ParamMaster>(); //見つけたオブジェクトのスクリプトを指定
                //script.open_value = Convert.ToSingle(value);
                //slider.value = script.open_value;

                //script = parammaster.GetComponent<ParamMaster>();

                if (lR == "L")
                {
                    if (ParamMaster.ServoCommand_L_s[servoNum] == 0)//サーボコマンドが0ならばパラムマスターの値を反映
                    {
                        ParamMaster.ServoAngles_L_diff[servoNum] = Convert.ToSingle(value);
                    }
                    else
                    {
                        ParamMaster.ServoAngles_L_UI[servoNum] = Convert.ToSingle(value);
                    }
                }
                else
                {
                    if (ParamMaster.ServoCommand_L_s[servoNum] == 0)//サーボコマンドが0ならばパラムマスターの値を反映
                    {
                        ParamMaster.ServoAngles_R_diff[servoNum] = Convert.ToSingle(value);
                    }
                    else
                    {
                        ParamMaster.ServoAngles_R_UI[servoNum] = Convert.ToSingle(value);
                    }
                }
            }
            else
            {
                Debug.Log("Input Error");
            }
            inputField.text = ""; //入力フォームのテキストを空にする
        }
    }
}