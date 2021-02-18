using UnityEngine;
using Hawkeye;

public class UnityLogger : ILog
{
    public void Output(string msg)
    {
        Debug.Log(msg);
    }

    public void Warn(string msg)
    {
        Debug.LogWarning(msg);
    }

    public void Error(string msg)
    {
        Debug.LogError(msg);
    }
}
