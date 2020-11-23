using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //---- Enums
    //----------
    public enum MainMenuState
    {
        None,
        Main,
        Host,
        Join,
        Lobby
    }

    //---- Variables
    //--------------
    [Header("Menus")]
    public MainMenu mainMenu;
    public HostMenu hostMenu;
    public JoinMenu joinMenu;

    private MenuBase menu;
    private MainMenuState state;

    // host
    private string gameName;
    private string hostName;

    // join
    private string ipaddress;
    private string playerName;

    //---- Start
    //----------
    private void Start()
    {
        // Setup UI actions
        MainSetup();
        HostSetup();
        JoinSetup();
        LobbySetup();

        // goto main menu
        SwitchState(MainMenuState.Main);
    }

    //---- State
    //----------
    private void SwitchState(MainMenuState state)
    {
        if(this.state == state)
        {
            return;
        }

        if(menu != null)
        {
            menu.Exit();
        }

        this.state = state;
        switch(state)
        {
            case MainMenuState.None:
                menu = null;
                return;
            case MainMenuState.Main:
                menu = mainMenu;
                break;
            case MainMenuState.Host:
                menu = hostMenu;
                break;
            case MainMenuState.Join:
                menu = joinMenu;
                break;
            case MainMenuState.Lobby:
                break;
        }
        menu.Enter();
    }

    //---- Back Button
    //----------------
    private void OnBack()
    {
        SwitchState(MainMenuState.Main);
    }

    //---- Main Menu
    //--------------
    private void MainSetup()
    {
        mainMenu.Init();
        mainMenu.hostButton.onClick.AddListener(OnHostGame);
        mainMenu.joinButton.onClick.AddListener(OnJoinGame);
    }

    private void OnHostGame()
    {
        SwitchState(MainMenuState.Host);
    }

    private void OnJoinGame()
    {
        SwitchState(MainMenuState.Join);
    }

    //---- Host Menu
    //--------------
    private void HostSetup()
    {
        hostMenu.Init();
        hostMenu.startLobby.onClick.AddListener(OnStartLobby);
    }

    private void OnStartLobby()
    {
        SwitchState(MainMenuState.Lobby);
    }

    //---- Join Menu
    //--------------
    private void JoinSetup()
    {
        joinMenu.Init();
        joinMenu.joinLobby.onClick.AddListener(OnJoinLobby);
    }

    private void OnJoinLobby()
    {
        SwitchState(MainMenuState.Lobby);
    }

    //---- Lobby Menu
    //---------------
    private void LobbySetup()
    {

    }

    //---- Validation Error
    //---------------------

    //---- Error
    //----------
}
