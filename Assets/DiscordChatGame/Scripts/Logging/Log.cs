using UnityEngine;

public static class Log
{
    public static string Timestamp()
    {
        return $"[{System.DateTime.Now:HH:mm:ss:FFF tt}] ";
    }
}