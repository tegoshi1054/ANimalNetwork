using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// TCP �ʐM���s���N���C�A���g���̃R���|�[�l���g
/// </summary>
public class Net : MonoBehaviour
{
    //================================================================================
    // �ϐ�
    //================================================================================
    // ���� IP �A�h���X�ƃ|�[�g�ԍ��̓T�[�o���Ɠ��ꂷ�邱��
    public string m_ipAddress = "127.0.0.1";
    public int m_port = 2001;

    private TcpClient m_tcpClient;
    private NetworkStream m_networkStream;
    private bool m_isConnection;

    private string m_message = "�s�J�`���E"; // �T�[�o�ɑ��M���镶����

    //================================================================================
    // �֐�
    //================================================================================
    /// <summary>
    /// ���������鎞�ɌĂяo����܂�
    /// </summary>
    private void Awake()
    {
        try
        {
            // �w�肳�ꂽ IP �A�h���X�ƃ|�[�g�ŃT�[�o�ɐڑ����܂�
            m_tcpClient = new TcpClient(m_ipAddress, m_port);
            m_networkStream = m_tcpClient.GetStream();
            m_isConnection = true;

            Debug.LogFormat("�ڑ�����");
        }
        catch (SocketException)
        {
            // �T�[�o���N�����Ă��炸�ڑ��Ɏ��s�����ꍇ�͂����ɗ��܂�
            Debug.LogError("�ڑ����s");
        }
    }

    /// <summary>
    /// GUI ��`�悷�鎞�ɌĂяo����܂�
    /// </summary>
    public void OnGUI()
    {
        // Awake �֐��Őڑ��Ɏ��s�����ꍇ�͂��̎|��\�����܂�
        if (!m_isConnection)
        {
            GUILayout.Label("�ڑ����Ă��܂���");
            return;
        }

        // �T�[�o�ɑ��M���镶����
        m_message = GUILayout.TextField(m_message);

        // ���M�{�^���������ꂽ��
        if (GUILayout.Button("���M"))
        {
            try
            {
                // �T�[�o�ɕ�����𑗐M���܂�
                var buffer = Encoding.UTF8.GetBytes(m_message);
                m_networkStream.Write(buffer, 0, buffer.Length);

                Debug.LogFormat("���M�����F{0}", m_message);
            }
            catch (Exception)
            {
                // �T�[�o���N�����Ă��炸���M�Ɏ��s�����ꍇ�͂����ɗ��܂�
                // SocketException �^���Ɨ�O�̃L���b�`���ł��Ȃ��悤�Ȃ̂�
                // Exception �^�ŗ�O���L���b�`���Ă��܂�
                Debug.LogError("���M���s");
            }
        }
    }

    /// <summary>
    /// �j�����鎞�ɌĂяo����܂�
    /// </summary>
    private void OnDestroy()
    {
        // �ʐM�Ɏg�p�����C���X�^���X��j�����܂�
        // Awake �֐��ŃC���X�^���X�̐����Ɏ��s���Ă���\��������̂�
        // null �������Z�q���g�p���Ă��܂�
        m_tcpClient?.Dispose();
        m_networkStream?.Dispose();

        Debug.Log("�ؒf");
    }
}