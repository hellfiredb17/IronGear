using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Inputs")]
    public TMP_InputField ipAddress;
    public TMP_InputField playerName;

    [Header("Button")]
    public Button joinLobby;

    //---- Interface
    //--------------
    public override void Init()
    {
        ipAddress.onValueChanged.AddListener(OnIpAddressChange);
    }

    public override void Enter()
    {
        ipAddress.text = string.Empty;
        playerName.text = string.Empty;
        base.Enter();
    }

    //---- IPAddress
    //--------------
    private void OnIpAddressChange(string text)
    {
        if(!ValidateIPAddress(ref text))
        {
            ipAddress.textComponent.color = Color.red;
            joinLobby.enabled = false;
        }
        else
        {
            ipAddress.textComponent.color = Color.black;
            joinLobby.enabled = true;
        }
    }

    //---- Validation
    //---------------
    private bool ValidateIPAddress(ref string text)
    {
        return System.Net.IPAddress.TryParse(text, out System.Net.IPAddress ip);        
    }
}
