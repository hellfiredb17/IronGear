using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hawkeye;

public class MenuManager : MonoBehaviour
{
    //---- Static Systems
    //-------------------
    public static MenuManager UIManager;

    //---- Menu States
    //----------------
    public enum Menu
    {
        // Shared
        None = 0,
        Main,
        HostJoin,
        Game,
        // Host
        HostSetup = 100,        
        LobbyHost,
        // Client
        ClientSetup = 200,
        LobbyClient,
    }

    [Flags]
    public enum Modal
    {
        // DEF - considered modal if it blocks interaction with the rest of the application
        // Think popups then
        None =          0,
        Settings =      1 << 0,
        WaitDialog =    1 << 1, // connecting, loading, etc
        DialogOK =      1 << 2,
        DialogYesNo =   1 << 3,
    }

    //---- Variables
    //--------------
    [Header("Menus")]
    public MainMenu mainMenu;
    public HostJoinMenu hostJoinMenu;

    [Header("Server Menus")]
    public HostMenu hostMenu;
    public LobbyHostMenu lobbyHostMenu;

    [Header("Client Menus")]
    public ClientMenu clientMenu;
    public LobbyClientMenu lobbyClientMenu;

    [Header("Modals")]

    private MenuBase currentMenu;
    private Menu current;

    //---- Properties
    //---------------
    public Menu Current => current;

    //---- Awake / Start
    //------------------
    public void Awake()
    {
        UIManager = this;
    }

    public void Start()
    {
        // better way to do this but for now
        lobbyHostMenu.Init();
        lobbyClientMenu.Init();

        Show(Menu.Main);
    }

    //---- Menu Interface
    //-------------------
    public void Show(Menu menu)
    {
        if(current == menu)
        {
            Debug.LogWarning("Tring to show same open menu");
            return;
        }

        // exit current
        if(currentMenu != null)
        {
            currentMenu.Exit();
        }

        // swap
        current = menu;
        switch(current)
        {
            case Menu.None:
                currentMenu = null;
                return;
            case Menu.Main:
                currentMenu = mainMenu;
                break;            
            case Menu.HostJoin:
                currentMenu = hostJoinMenu;
                break;
            // Host
            case Menu.HostSetup:
                currentMenu = hostMenu;
                break;
            case Menu.LobbyHost:
                currentMenu = lobbyHostMenu;
                break;
            // Client
            case Menu.ClientSetup:
                currentMenu = clientMenu;
                break;
            case Menu.LobbyClient:
                currentMenu = lobbyClientMenu;
                break;
        }

        // enter new
        currentMenu.Enter();
    }

    public void Hide()
    {
        if(current == Menu.None || currentMenu == null)
        {
            return;
        }
        currentMenu.Exit();
        currentMenu = null;
        current = Menu.None;
    }

    public T GetCurrentMenu<T>() where T : MenuBase
    {
        return currentMenu as T;
    }

    public T GetMenu<T>(Menu menu) where T : MenuBase
    {
        switch (menu)
        {            
            // Shared
            case Menu.Main:
                return mainMenu as T;                
            case Menu.HostJoin:
                return hostJoinMenu as T;
            case Menu.Game:
                return null;
            // Host
            case Menu.HostSetup:
                return hostMenu as T;
            case Menu.LobbyHost:
                return lobbyHostMenu as T;
            // Client
            case Menu.ClientSetup:
                return clientMenu as T;
            case Menu.LobbyClient:
                return lobbyClientMenu as T;
        }
        return null;
    }

    //---- Modals Interface
    // TODO - able to add different Modals 
    // as a stack over current menus
    //---------------------
    public void ShowModal(Modal modal)
    {
    }

    public T GetModal<T>(Modal modal)
    {
        return default(T);
    }

    public void HideModal(Modal modal)
    {
    }
}
