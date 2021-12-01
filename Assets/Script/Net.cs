using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// TCP 通信を行うクライアント側のコンポーネント
/// </summary>
public class Net : MonoBehaviour
{
    //================================================================================
    // 変数
    //================================================================================
    // この IP アドレスとポート番号はサーバ側と統一すること
    public string m_ipAddress = "127.0.0.1";
    public int m_port = 2001;

    private TcpClient m_tcpClient;
    private NetworkStream m_networkStream;
    private bool m_isConnection;

    private string m_message = "ピカチュウ"; // サーバに送信する文字列

    //================================================================================
    // 関数
    //================================================================================
    /// <summary>
    /// 初期化する時に呼び出されます
    /// </summary>
    private void Awake()
    {
        try
        {
            // 指定された IP アドレスとポートでサーバに接続します
            m_tcpClient = new TcpClient(m_ipAddress, m_port);
            m_networkStream = m_tcpClient.GetStream();
            m_isConnection = true;

            Debug.LogFormat("接続成功");
        }
        catch (SocketException)
        {
            // サーバが起動しておらず接続に失敗した場合はここに来ます
            Debug.LogError("接続失敗");
        }
    }

    /// <summary>
    /// GUI を描画する時に呼び出されます
    /// </summary>
    public void OnGUI()
    {
        // Awake 関数で接続に失敗した場合はその旨を表示します
        if (!m_isConnection)
        {
            GUILayout.Label("接続していません");
            return;
        }

        // サーバに送信する文字列
        m_message = GUILayout.TextField(m_message);

        // 送信ボタンが押されたら
        if (GUILayout.Button("送信"))
        {
            try
            {
                // サーバに文字列を送信します
                var buffer = Encoding.UTF8.GetBytes(m_message);
                m_networkStream.Write(buffer, 0, buffer.Length);

                Debug.LogFormat("送信成功：{0}", m_message);
            }
            catch (Exception)
            {
                // サーバが起動しておらず送信に失敗した場合はここに来ます
                // SocketException 型だと例外のキャッチができないようなので
                // Exception 型で例外をキャッチしています
                Debug.LogError("送信失敗");
            }
        }
    }

    /// <summary>
    /// 破棄する時に呼び出されます
    /// </summary>
    private void OnDestroy()
    {
        // 通信に使用したインスタンスを破棄します
        // Awake 関数でインスタンスの生成に失敗している可能性もあるので
        // null 条件演算子を使用しています
        m_tcpClient?.Dispose();
        m_networkStream?.Dispose();

        Debug.Log("切断");
    }
}