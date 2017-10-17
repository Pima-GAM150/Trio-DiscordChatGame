using Discord.WebSocket;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Discord;
using System;
using System.Net;

public class DiscordLauncher : MonoBehaviour
{
    /// <summary>
    /// The active Discord Socket Client. Returns null if no client exists.
    /// </summary>
    public DiscordSocketClient Client
    {
        get
        {
            if (_client == null)
            {
                Debug.LogError($"{LogHelpers.FormatTimestamp()} DiscordSocketClient accessed before it was Initialized.");
                return null;
            }
            return _client;
        }
        private set
        {
            _client = value;
        }
    }

    public static DiscordLauncher ActiveLauncher;
    private static DiscordSocketClient _client;

    public void Awake()
    {
        if (ActiveLauncher != null)
        {
            Debug.Log($"{LogHelpers.FormatTimestamp()} Destroying {this} on {name}. Launcher is already set.");
            Destroy(this);
            return;
        }
        ActiveLauncher = this;
        DontDestroyOnLoad(this);
    }

    public void StartDiscordBot(string token)
    {
        // Start async context
        Debug.Log($"{LogHelpers.FormatTimestamp()} Starting Async Task to launch Discord Bot");
        StartBotAsync(token).GetAwaiter().GetResult();
    }

    public async Task StartBotAsync(string token)
    {
        // Disable SSL Certificate Validation.
        // This is technically wrong and another way should be found, but i have no clue where to begin looking since it's such an obscure problem.
        ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };

        //Create our client config
        var socketConfig = new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose
        };

        // create our client
        Client = new DiscordSocketClient(socketConfig);
        Debug.Log($"{LogHelpers.FormatTimestamp()} Created Client");
        Client.Log += ConvertDiscordLog;

        Debug.Log($"{LogHelpers.FormatTimestamp()} Awaiting Login");
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        Debug.Log($"{LogHelpers.FormatTimestamp()} Keeping Task Alive");
        // keep task alive until this is closed.
        await Task.Delay(-1);
    }

    private async void OnDestroy()
    {
        // make sure our client is destroyed.
        await Client.LogoutAsync();
        Client = null;
    }

    /// <summary>
    /// Converts a discord.net LogMessage object to a unity Debug.Log message.
    /// </summary>
    private Task ConvertDiscordLog(LogMessage logMsg)
    {
        var msg = $"{LogHelpers.FormatTimestamp()} Discord-{logMsg.Severity}: {logMsg.Message}";

        if (logMsg.Exception != null && logMsg.Exception.Message != string.Empty)
            msg += $"[Exception] {logMsg.Exception.Message}";

        switch (logMsg.Severity)
        {
            case LogSeverity.Critical:
                Debug.LogAssertion(msg);
                break;

            case LogSeverity.Error:
                Debug.LogError(msg);
                break;

            case LogSeverity.Warning:
                Debug.LogWarning(msg);
                break;

            case LogSeverity.Info:
                Debug.Log(msg);
                break;

            case LogSeverity.Verbose:
                Debug.Log(msg);
                break;

            case LogSeverity.Debug:
                Debug.Log(msg);
                break;
        }
        return Task.CompletedTask;
    }
}