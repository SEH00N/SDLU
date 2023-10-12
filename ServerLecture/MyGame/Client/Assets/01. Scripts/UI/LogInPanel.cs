using Packets;
using TMPro;
using UnityEngine;

public class LogInPanel : MonoBehaviour
{
    private TMP_InputField nicknameField;

    private void Awake()
    {
        nicknameField = transform.Find("NameInput")?.GetComponent<TMP_InputField>();
    }

	public void LogIn()
    {
        C_LogInPacket packet = new C_LogInPacket();
        packet.nickname = nicknameField.text;

        NetworkManager.Instance.Send(packet);
    }
}
