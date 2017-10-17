using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    /// <summary>
    /// root object for the Discord Token screen.
    /// </summary>
    public GameObject TokenScreen;

    /// <summary>
    /// Text object to display push notifications on.
    /// </summary>
    public Text InfoText;

    /// <summary>
    /// root object for the main menu page.
    /// </summary>
    public GameObject MainPage;

    /// <summary>
    /// Time to display a push notification in MS;
    /// </summary>
    [Tooltip("Time to display push notifications in MS")]
    public int PushNotificationTime;

    private InputField _tokenField;

    public void Start()
    {
        _tokenField = TokenScreen.GetComponentInChildren<InputField>();
        if (_tokenField == null)
        {
            Debug.LogError($"{LogHelpers.FormatTimestamp()} No InputField on TokenScreen!");
        }

        if (string.IsNullOrEmpty(ConfigManager.ActiveManager.DiscordConfig.Token))
        {
            MainPage.SetActive(false);
        }
        else
            TokenScreen.SetActive(false);

        InfoText.gameObject.SetActive(false);
    }

    public async void OnTokenScreenCommit()
    {
        _tokenField.DeactivateInputField();
        var ctx = DiscordLauncher.ActiveLauncher;
        Debug.Log($"{LogHelpers.FormatTimestamp()} Sending Token to DiscordLauncher");
        await ctx.StartBotAsync(_tokenField.text);
        var client = ctx.Client;

        // subscribe to connection events.
        Debug.Log($"{LogHelpers.FormatTimestamp()} Subscribing to Discord Bot Events");
        client.LoggedIn += Client_Connected;
        client.Log += DiscordLogUpdate;
    }

    private async Task DiscordLogUpdate(LogMessage msg)
    {
        if (msg.Severity > LogSeverity.Verbose)
            await PushNotification($"[{msg.Severity}]: {msg.Message}");
    }

    private async Task Client_Connected()
    {
        await PushNotification("Discord Client Successfully Connected!");
        var client = DiscordLauncher.ActiveLauncher.Client;

        // unsubscribe from events since we will no longer need them.
        client.LoggedIn -= Client_Connected;
        client.Log -= DiscordLogUpdate;

        // switch menu screen.
        TokenScreen.SetActive(false);
        MainPage.SetActive(true);
    }

    private async Task PushNotification(string msg)
    {
        Debug.Log($"[Push Notification]: {msg}");
        InfoText.text = msg;
        InfoText.gameObject.SetActive(true);

        await Task.Delay(PushNotificationTime);

        InfoText.gameObject.SetActive(false);
    }
}