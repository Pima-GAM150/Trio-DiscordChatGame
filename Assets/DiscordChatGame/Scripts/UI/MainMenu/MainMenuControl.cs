#pragma warning disable CS4014

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using UnityEngine;
using UnityEngine.UI;
using DSharpPlus;

public class MainMenuControl : MonoBehaviour
{
    /// <summary>
    /// root object for the Discord Token screen.
    /// </summary>
    public GameObject TokenScreen;

    public PushNotification PushNotificationObj;

    /// <summary>
    /// root object for the main menu page.
    /// </summary>
    public GameObject MainPage;

    /// <summary>
    /// Time to display a push notification in MS;
    /// </summary>
    [Tooltip("Time to display push notifications in MS")]
    public int PushNotificationTime;

    [Tooltip("Color to use on push notifications from Discord-Info")]
    public Color PushColorInfo = Color.white;

    [Tooltip("Color to use on push notifications from Discord-Error")]
    public Color PushColorError = Color.red;

    [Tooltip("Color to use on push notifications from Discord-Warning")]
    public Color PushColorWarning = Color.yellow;

    private InputField _tokenField;

    public void Start()
    {
        _tokenField = TokenScreen.GetComponentInChildren<InputField>();
        if (_tokenField == null)
        {
            Debug.LogError($"{Log.Timestamp()} No InputField on TokenScreen!");
        }

        if (string.IsNullOrEmpty(ConfigManager.ActiveManager.DiscordConfig.Token))
        {
            MainPage.SetActive(false);
        }
        else
            TokenScreen.SetActive(false);
    }

    public void OnTokenScreenCommit()
    {
        _tokenField.DeactivateInputField();
        var ctx = DiscordChatActor.Instance;
        Debug.Log($"{Log.Timestamp()} Sending Token to DiscordLauncher");

        ctx.CreateClient(_tokenField.text);
        ctx.OnLogMessage += DiscordLogUpdate;
        ctx.Run();
    }

    private void DiscordLogUpdate(string msg, LogLevel level)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Add(() => DiscordLogUpdate(msg, level));
            return;
        }
        //Debug.Log($"Message Recieved by {this.ToString()}");
        switch (level)
        {
            case LogLevel.Critical:
                PushNotification(msg, PushColorError);
                break;

            case LogLevel.Error:
                PushNotification(msg, PushColorError);
                break;

            case LogLevel.Warning:
                PushNotification(msg, PushColorWarning);
                break;

            case LogLevel.Info:
                PushNotification(msg, PushColorInfo);
                break;
        }
    }

    private void Client_Connected()
    {
        PushNotification("Discord Client Successfully Connected!", PushColorInfo);

        // switch menu screen.
        TokenScreen.SetActive(false);
        MainPage.SetActive(true);
    }

    private void PushNotification(string msg, Color color)
    {
        Debug.Log($"{Log.Timestamp()} [Push Notification]: {msg}");

        PushNotificationObj.Add(msg, color);
    }
}