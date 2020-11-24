using UnityEngine;

public abstract class MenuBase : MonoBehaviour
{
    //---- Interface
    //--------------
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
