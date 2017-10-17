using System;

public static class LogHelpers
{
    /// <summary>
    /// Returns the current time formatted as mm:ss:FFFF, ie 5:23:24 PM
    /// </summary>
    /// <returns></returns>
    public static string FormatTimestamp()
    {
        return $"[{DateTime.Now:HH:mm:ss:FF tt}]";
    }
}