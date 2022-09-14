using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TandaOp : MonoBehaviour
{
    private GameObject parent;
    private GameObject parammaster;

    private ParamMaster script;

    public float roll;
    public float pitch;
    public float yaw;


    void Start()
    {
        //parammaster = GameObject.Find("ParamMaster");
    }
    void FixedUpdate()
    {
        //script = parammaster.GetComponent<ParamMaster>();
        roll = ParamMaster.ACxyz_GYxyz_CPxyz_TP_RPY[10];
        pitch = ParamMaster.ACxyz_GYxyz_CPxyz_TP_RPY[11];
        yaw = ParamMaster.ACxyz_GYxyz_CPxyz_TP_RPY[12];

        Vector3 rot = this.transform.localEulerAngles;

        rot.x = pitch;
        rot.y = yaw;
        rot.z = roll;
        this.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }

    void Update()
    {

    }
}