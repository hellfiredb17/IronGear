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
    public enum State
    {
        None,
        Main,
        HostJoin,
        HostSetup,
        LobbyList,
        Lobby,
        Game
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
    public HostMenu hostMenu;
    public JoinMenu joinMenu;
    public LobbyHostMenu lobbyMenu;

    [Header("Modals")]

    private MenuBase currentMenu;
    private State current;

    //---- Properties
    //---------------
    public State Current => current;

    //---- Awake / Start
    //------------------
    public void Awake()
    {
        UIManager = this;
    }

    public void Start()
    {
        Show(State.Main);
    }

    //---- Menu Interface
    //-------------------
    public void Show(State menu)
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
            case State.None:
                currentMenu = null;
                return;
            case State.Main:
                currentMenu = mainMenu;
                break;
            case State.HostJoin:
                currentMenu = hostJoinMenu;
                break;
            case State.HostSetup:
                currentMenu = hostMenu;
                break;
            case State.LobbyList:
                currentMenu = joinMenu;
                break;
            case State.Lobby:
                currentMenu = lobbyMenu;
                break;
        }

        // enter new
        currentMenu.Enter();
    }

    public void Hide()
    {
        if(current == State.None || currentMenu == null)
        {
            return;
        }
        currentMenu.Exit();
        currentMenu = null;
        current = State.None;
    }

    public T GetCurrentMenu<T>() where T : MenuBase
    {
        return currentMenu as T;
    }

    public T GetMenu<T>(State menu) where T : MenuBase
    {
        switch (menu)
        {            
            case State.Main:
                return mainMenu as T;                
            case State.HostJoin:
                return hostJoinMenu as T;
            case State.HostSetup:
                return hostMenu as T;                
            case State.LobbyList:
                return joinMenu as T;
            case State.Lobby:
                return lobbyMenu as T;
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
