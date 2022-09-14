using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class SyncVRChat : MonoBehaviour
{
    // Start is called before the first frame update

    public Toggle toggle;
    public ParamMaster pm;

    void Start()
    {
        Application.targetFrameRate = 30; //60FPS‚Éİ’è
    }

    // Update is called once per frame
    void Update()
    {
        int midiChannel = 0;
        int noteNumber = 0;
        int velocity = 0;
        int send_value = 0;

        if (toggle.isOn == true)
        {
            //ƒgƒOƒ‹‚ªƒIƒ“‚È‚ç“¯Šú‚·‚é

            for (int i = 0; i < 11; i++)
            {
                send_value = ((int)(-pm.ServoAngles_L_disp[i] * 10)) + 1800;
                if (i == 0) send_value = ((int)(-pm.ServoAngles_R_disp[i] * 10)) + 1800;    // 0‚Ì‚¾‚¯LR‚Ì“ü‚ê‘Ö‚¦
                if (i == 2) send_value = ((int)(pm.ServoAngles_L_disp[i] * 10)) + 1800;
                if (i == 6) send_value = ((int)(pm.ServoAngles_L_disp[i] * 10)) + 1800;
                if (i == 10) send_value = ((int)(pm.ServoAngles_L_disp[i] * 10)) + 1800;

                velocity = send_value % 128;
                noteNumber = (send_value / 128) + ((i % 4) * 32);
                midiChannel = i / 4;

                SendNoteOn((MidiChannel)midiChannel, noteNumber, ((float)velocity /127.0f));
            }

            for (int i = 0; i < 11; i++)
            {
                send_value = ((int)(-pm.ServoAngles_R_disp[i] * 10)) + 1800;
                if (i == 0) send_value = ((int)(-pm.ServoAngles_L_disp[i] * 10)) + 1800;    // 0‚Ì‚¾‚¯LR‚Ì“ü‚ê‘Ö‚¦
                if (i == 5) send_value = ((int)(pm.ServoAngles_R_disp[i] * 10)) + 1800;

                velocity = send_value % 128;
                noteNumber = (send_value / 128) + ((i+11) % 4) * 32;
                midiChannel = (i + 11) / 4;

                SendNoteOn((MidiChannel)midiChannel, noteNumber, ((float)velocity / 127.0f));
            }
        }
    }


    public static void SendNoteOn(MidiChannel channel, int noteNumber, float velocity)
    {
        int cn = Mathf.Clamp((int)channel, 0, 15);
        noteNumber = Mathf.Clamp(noteNumber, 0, 127);
        velocity = Mathf.Clamp(127.0f * velocity, 0.0f, 127.0f);
        MidiBridge.instance.Send(0x90 + cn, noteNumber, (int)velocity);
    }

    public static void SendNoteOff(MidiChannel channel, int noteNumber)
    {
        int cn = Mathf.Clamp((int)channel, 0, 15);
        noteNumber = Mathf.Clamp(noteNumber, 0, 127);
        MidiBridge.instance.Send(0x80 + cn, noteNumber, 0);
    }

    public static void SendControlChange(MidiChannel channel, int controlNumber, float value)
    {
        int cn = Mathf.Clamp((int)channel, 0, 15);
        controlNumber = Mathf.Clamp(controlNumber, 0, 127);
        value = Mathf.Clamp(127.0f * value, 0.0f, 127.0f);
        MidiBridge.instance.Send(0xb0 + cn, controlNumber, (int)value);
    }

}
