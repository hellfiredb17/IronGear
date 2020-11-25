using UnityEngine;

public abstract class MenuBase : MonoBehaviour
{
    //---- Variable
    //-------------
    protected MenuManager menuManager;

    //---- Interface
    //--------------
    protected virtual void Start()
    {
        menuManager = MenuManager.UIManager;
        Init();
    }

    public virtual void Init() 
    {
    }

    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }
}
