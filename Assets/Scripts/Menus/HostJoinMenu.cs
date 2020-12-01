using UnityEngine;
using UnityEngine.UI;

public class HostJoinMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Buttons")]
    public Button HostButton;
    public Button JoinButton;

    //----- Interface
    //---------------
    public override void Init()
    {
        HostButton.onClick.AddListener(OnHostPress);
        JoinButton.onClick.AddListener(OnJoinPress);
    }

    //---- Buttons
    //------------
    private void OnHostPress()
    {
        menuManager.Show(MenuManager.Menu.HostSetup);
    }

    private void OnJoinPress()
    {
        menuManager.Show(MenuManager.Menu.ClientSetup);
    }
}
