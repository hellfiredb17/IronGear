using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Inputs")]
    public TMP_InputField gameName;
    public TMP_InputField playerName;

    [Header("Button")]
    public Button startLobby;

    //---- Interface
    //--------------
    public override void Enter()
    {
        gameName.text = string.Empty;
        playerName.text = string.Empty;
        base.Enter();
    }
}
