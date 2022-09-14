//meridian_20220408_unity_UDP_Resvhandler.cs
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class UDP_ResvHandler : MonoBehaviour //UDP受信用

{
    public string dataCheck = "check";//データチェック表示用
    public static string dataCheck_tmp = "check";//データチェック表示用
    public short sum_recv = 0;//データチェック表示用
    static short sum_revb_tmp = 0;//データチェック表示用
    public short sum_culc = 0;//データチェック表示用
    static short sum_culc_tmp = 0;//データチェック表示用

    const int RECEIVE_PORT = 22222;//受信に使うポートの番号
    static UdpClient udp_receive;//UDP受信を使う準備
    Thread thread;//スレッドを使う準備

    const int MSG_SIZE = 90;//送受信するshort型データの個数
    static short[] r_packet_short = new short[MSG_SIZE];//データ格納用のshort型変数（２バイト）
    public int[] r_meridim_monitor = new int[MSG_SIZE];//受信データインスペクタ表示用
    private static bool flag_resv_loop = true;

    void Start()
    {
        //FixedUpdateの間隔を設定する
        Time.fixedDeltaTime = 0.01f;

        Thread_UDP_resv();

        //UDP関連の諸設定
        //udp_receive = new UdpClient(RECEIVE_PORT);
        ////udp_receive.Client.ReceiveTimeout = 1000;//UDP通信のタイムアウト設定
        //thread = new Thread(new ThreadStart(ThreadMethod));//受信用スレッドを準備
        //thread.Start();//受信用スレッドを開始

        dataCheck = "check";
    }

    void FixedUpdate()//UDP受信データをPramMasterに反映
    {
        //[1]受信データをインスペクター表示用配列に転記
        //<1-1>//受信データをインスペクター表示用配列に転記
        for (int i = 0; i < MSG_SIZE; i++)
        {
            r_meridim_monitor[i] = ParamMaster.r_meridim[i];//受信データをインスペクター表示用配列に転記
        }

        //<1-2>受信センサデータのparamMaster反映処理

        for (int i = 0; i < 13; i++)
        {
            ParamMaster.ACxyz_GYxyz_CPxyz_TP_RPY[i] = (float)ParamMaster.r_meridim[i + 2] / 100;
        }

        //<1-2b>//データスルーパス。受信データを送信データに一旦全転記
        for (int i = 0; i < MSG_SIZE; i++)
        {
            ParamMaster.s_meridim[i] = ParamMaster.r_meridim[i];//受信データを送信データに全転記
        }

        //<1-3>受信サーボ値のparamMaster反映処理
        for (int i = 0; i < 15; i++)
        {
            if (ParamMaster.ServoCommand_L_s[i] == 0)//サーボコマンドが0なら受信値反映
            {
                ParamMaster.ServoAngles_L_diff[i] = (float)ParamMaster.r_meridim[i * 2 + 21] / 100;
            }

            if (ParamMaster.ServoCommand_R_s[i] == 0)//サーボコマンドが0なら受信値反映
            {
                ParamMaster.ServoAngles_R_diff[i] = (float)ParamMaster.r_meridim[i * 2 + 51] / 100;
            }
        }
    }

    void Update()//UDP受信データをインスペクターに反映＆チェックサム実行
    {

        for (int i = 0; i < MSG_SIZE; i++)
        {
            r_meridim_monitor[i] = r_packet_short[i];//受信データをインスペクター表示用配列に転記
        }
    }


    void OnApplicationQuit()//アプリ終了時の処理.
    {
        flag_resv_loop = false;
        udp_receive.Close();
        //thread.Abort();
    }

    //private static void ThreadMethod()//受信スレッド用の関数
    void Thread_UDP_resv()//受信スレッド用の関数

    {
        udp_receive = new UdpClient(RECEIVE_PORT);
        Task.Run(() =>
        {

            //udp_receive = new UdpClient(RECEIVE_PORT);

            while (flag_resv_loop)
            {
                IPEndPoint remoteEP = null;
                byte[] data = udp_receive.Receive(ref remoteEP);

                //[1]パケットサイズをチェックし、受信バッファに転記
                if (data.Length == MSG_SIZE * 2) // 
                {
                    for (int i = 0; i < MSG_SIZE; i++)
                    {
                        r_packet_short[i] = BitConverter.ToInt16(data, i * 2);
                    }
                }

                long checksum = 0;
                for (int i = 0; i < MSG_SIZE - 1; i++)
                {
                    checksum += r_packet_short[i];
                }
                checksum = ~checksum;

                sum_revb_tmp = r_packet_short[MSG_SIZE - 1];//データチェック表示用
                sum_culc_tmp = (short)checksum;//データチェック表示用

                //[2]チェックサムを計算しキープ
                if (r_packet_short[MSG_SIZE - 1] == (short)checksum)
                {
                    dataCheck_tmp = "OK.";

                    //[3]チェックサムが判定OKなら
                    for (int i = 0; i < MSG_SIZE; i++)
                    {
                        ParamMaster.r_meridim[i] = r_packet_short[i];//受信バッファから受信データに格納
                    }
                }
                else
                {
                    dataCheck_tmp = "**NG**";
                }

                //Thread.Sleep(1);
            }
        });
    }
}
