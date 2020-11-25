using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    //---- Variables
    //--------------
    [Header("Buttons")]    
    public Button StartButton;

    //---- Interface
    //--------------
    public override void Init()
    {
        StartButton.onClick.AddListener(OnStartPress);
    }

    //---- Button
    //-----------
    private void OnStartPress()
    {
        menuManager.Show(MenuManager.State.HostJoin);
    }
}
