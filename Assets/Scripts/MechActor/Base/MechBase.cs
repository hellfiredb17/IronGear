using UnityEngine;
using Rigs;

/// <summary>
/// GameObject container/wrapper for Mech Base MVC
/// </summary>
public class MechBase : MonoBehaviour
{
    //---- Public
    //-----------
    public MechBaseModel Model;
    public MechBaseView View;
    public MechBaseController Controller;
}
