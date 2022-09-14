//meridian_20220408_unity_UDP_Sendhandler.cs
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

public class UDP_SendHandler : MonoBehaviour //UDP送信用

{
    const string HOST = "192.168.1.1";//送信先(ESP32)のIPアドレス
    const int SEND_PORT = 22224;//送信用ポート設定
    static UdpClient udp_send;//UDP送信を使う準備

    const int MSG_SIZE = 90;//送受信するshort型データの個数
    public int[] r_meridim_monitor = new int[MSG_SIZE];//受信データインスペクタ表示用

    public static byte[] s_packet_byte = new byte[MSG_SIZE * 2];//データ送信用のchar型変数（１バイト）
    public static short[] s_packet_short = new short[MSG_SIZE];//データ送信格納用のshort型変数（２バイト）
    public static int[] s_meridim_monitor = new int[MSG_SIZE];//送信データインスペクタ表示用

    private static bool flag_send_loop = true;
    //private static bool flag_udp_send_ok = true;
    Thread thread;//スレッドを使う準備

    void Start()
    {
        //UDP送信スレッドを開始
        //Thread_UDP_send();
        udp_send = new UdpClient();
        udp_send.Connect(HOST, SEND_PORT);
    }


    void FixedUpdate()//FixedUpdateではなにもしない
    {
        while (ParamMaster.s_meridim_udp_write_flag)//s_meridim_udpが書き込みアクセス中なら終了まで待つ
        {
            Delayer(0.001f);
        }

        if (ParamMaster.s_udp_queue) //転記完了ならば
        {
            ParamMaster.s_meridim_udp_read_flag = true;//読み取りフラグ揚げ

            //送信UDPデータ作成処理

            //[1]
            //<1-1>s_packet_shortにs_meridimを転記
            for (int i = 0; i < MSG_SIZE; i++)
            {
                s_packet_short[i] = ParamMaster.s_meridim_udp[i];
            }

            //<1-2>トグルスイッチのオンオフ（サーボをUnityから動作させるかどうか）を取得
            for (int i = 0; i < 15; i++)
            {
                s_packet_short[(i * 2) + 20] = (short)(ParamMaster.ServoCommand_L_s[i]);//受信サーボコマンドのParamserver反映
                s_packet_short[(i * 2) + 50] = (short)(ParamMaster.ServoCommand_R_s[i]);//受信サーボコマンドのParamserver反映
            }

            //[2]送信データを作成
            //<2-1>チェックサムの作成
            int checksum = 0;
            for (int i = 0; i < MSG_SIZE - 1; i++)
            {
                checksum += s_packet_short[i];
            }
            checksum = ~checksum;
            s_packet_short[MSG_SIZE - 1] = (short)checksum;

            //<2-2>ショート型をバイトに変換
            for (int i = 0; i < MSG_SIZE; i++)
            {
                byte[] byteArray = BitConverter.GetBytes(s_packet_short[i]);
                s_packet_byte[i * 2] = (byte)byteArray[0];
                s_packet_byte[i * 2 + 1] = (byte)byteArray[1];
                //<2-3>送信データをインスペクタ表示用配列に反映
                s_meridim_monitor[i] = s_packet_short[i];
            }

            ParamMaster.s_meridim_udp_read_flag = false;//読み取りフラグ下げ
            ParamMaster.s_udp_queue = false; //s_meridim_udpの制御キュー（転記完了で+1(1)、送信完了で-1(0)）
        }

        //[3]送信を実行
        udp_send.Send(s_packet_byte, MSG_SIZE * 2);
        //Thread.Sleep(1);
        //flag_udp_send_ok = true;

    }
    IEnumerator Delayer(float num)
    {
        yield return new WaitForSeconds(num);
    }


    void Update()//UDP送信データをインスペクターに反映
    {
        for (int i = 0; i < MSG_SIZE; i++)
        {
            s_meridim_monitor[i] = s_packet_short[i];//受信データをインスペクター表示用配列に転記
        }
    }

    void OnApplicationQuit()//アプリ終了時の処理.
    {
        flag_send_loop = false;//UDP受信スレッドのループフラグを下げてループを終了させる
        udp_send.Close();//UDPの送信を終了
    }

}

/*
    //private static void ThreadMethod()//送信スレッド用の関数
    void Thread_UDP_send()//送信スレッド用の関数

    {
        //UDP関連の諸設定
        udp_send = new UdpClient();
        udp_send.Connect(HOST, SEND_PORT);

        Task.Run(() =>//以下送信のループ処理
        {

            while (flag_send_loop)//UDP送信スレッドのループフラグが揚がっている間はループ
            {
                //flag_udp_send_ok = false;

                while (ParamMaster.s_meridim_udp_write_flag)//s_meridim_udpが書き込みアクセス中なら終了まで待つ
                {
                    Thread.Sleep(1);
                }

                if (ParamMaster.s_udp_queue) //転記完了ならば
                {
                    ParamMaster.s_meridim_udp_read_flag = true;//読み取りフラグ揚げ

                    //送信UDPデータ作成処理

                    //[1]
                    //<1-1>s_packet_shortにs_meridimを転記
                    for (int i = 0; i < MSG_SIZE; i++)
                    {
                        s_packet_short[i] = ParamMaster.s_meridim_udp[i];
                    }

                    //<1-2>トグルスイッチのオンオフ（サーボをUnityから動作させるかどうか）を取得
                    for (int i = 0; i < 15; i++)
                    {
                        s_packet_short[(i * 2) + 20] = (short)(ParamMaster.ServoCommand_L_s[i]);//受信サーボコマンドのParamserver反映
                        s_packet_short[(i * 2) + 50] = (short)(ParamMaster.ServoCommand_R_s[i]);//受信サーボコマンドのParamserver反映
                    }

                    //[2]送信データを作成
                    //<2-1>チェックサムの作成
                    int checksum = 0;
                    for (int i = 0; i < MSG_SIZE - 1; i++)
                    {
                        checksum += s_packet_short[i];
                    }
                    checksum = ~checksum;
                    s_packet_short[MSG_SIZE - 1] = (short)checksum;

                    //<2-2>ショート型をバイトに変換
                    for (int i = 0; i < MSG_SIZE; i++)
                    {
                        byte[] byteArray = BitConverter.GetBytes(s_packet_short[i]);
                        s_packet_byte[i * 2] = (byte)byteArray[0];
                        s_packet_byte[i * 2 + 1] = (byte)byteArray[1];
                        //<2-3>送信データをインスペクタ表示用配列に反映
                        s_meridim_monitor[i] = s_packet_short[i];
                    }

                    ParamMaster.s_meridim_udp_read_flag = false;//読み取りフラグ下げ
                    ParamMaster.s_udp_queue = false; //s_meridim_udpの制御キュー（転記完了で+1(1)、送信完了で-1(0)）
                }

                //[3]送信を実行
                udp_send.Send(s_packet_byte, MSG_SIZE * 2);
                //Thread.Sleep(1);
                //flag_udp_send_ok = true;

            }
        });
    }
}

*/