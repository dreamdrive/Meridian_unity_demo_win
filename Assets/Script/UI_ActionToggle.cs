using UnityEngine;
using UnityEngine.UI;

public class UI_ActionToggle : MonoBehaviour
{
    // Start is called before the first frame update
    //private GameObject parent;
    private Toggle toggle;
    private GameObject parammaster;
    private ParamMaster script;


    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnToggleChanged()
    {
        //parammaster = GameObject.Find("ParamMaster");
        //script = parammaster.GetComponent<ParamMaster>();

        if (toggle.isOn == true)
        {
            for (int i = 0; i < 15; i++)
            {
                ParamMaster.ServoCommand_L_s[i] = 1;//
                ParamMaster.ServoCommand_R_s[i] = 1;//
            }
            ParamMaster.actionToggle = true;
            Debug.Log("'action toggle' on");
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                ParamMaster.ServoCommand_L_s[i] = 0;//
                ParamMaster.ServoCommand_R_s[i] = 0;//
            }
            ParamMaster.actionToggle = false;
            Debug.Log("'action toggle' off");
        }
    }
}