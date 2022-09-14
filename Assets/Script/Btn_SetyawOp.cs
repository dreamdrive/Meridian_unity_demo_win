using UnityEngine;
using UnityEngine.UI;

public class Btn_SetyawOp : MonoBehaviour
{
    // Start is called before the first frame update
    //private GameObject parent;
    private Button SetYawBtn;
    private GameObject parammaster;
    private ParamMaster script;


    void Start()
    {
        SetYawBtn = GetComponent<Button>();
    }

    public void OnClick()
    {


        //parammaster = GameObject.Find("ParamMaster");
        //script = parammaster.GetComponent<ParamMaster>();


        ParamMaster.mastercommand = 10002;//


        Debug.Log("Send command 10002 : Set Yaw");
    }
}