using UnityEngine;

public abstract class MenuBase : MonoBehaviour
{
    //---- Interface
    //--------------
    public abstract void Init();

    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }
}
