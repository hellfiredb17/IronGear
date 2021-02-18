using System;
using Hawkeye;

public class DediLogger : ILog
{
    public void Output(string msg)
    {
        Console.WriteLine($"<color=black>{msg}</color>");
    }

    public void Warn(string msg)
    {
        Console.WriteLine($"<color=yellow>{msg}</color>");
    }

    public void Error(string msg)
    {
        Console.WriteLine($"<color=red>{msg}</color>");
    }
}
