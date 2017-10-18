using DSharpPlus;
using DSharpPlus.Net.WebSocket;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using DSharpPlus.EventArgs;
using System.Net;

public class DiscordChatActor : MonoBehaviour
{
    public static DiscordChatActor Instance { get; private set; }
    public DiscordClient Client { get; private set; }

    public async Task Run(string token)
    {
        // Disable SSL Certificate Validation
        // see: https://dsharpplus.emzi0767.com/articles/hosting_rpi.html#method-4-run-your-bot-using-mono
        ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;
        Client = new DiscordClient(GenerateConfig(token));
        Client.SetWebSocketClient<WebSocketSharpClient>();

        Client.Ready += Client_Ready;
        Client.DebugLogger.LogMessageReceived += DebugLogger_LogMessageReceived;
        Client.ClientErrored += Client_ClientErrored;

        await Client.ConnectAsync();
    }

    public async Task Stop()
    {
        await Client.DisconnectAsync();
    }

    private Task Client_ClientErrored(ClientErrorEventArgs e)
    {
        Debug.LogError($"{Log.Timestamp()} Discord-ClientErrored: {e.Exception}");
        return Task.CompletedTask;
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private async void OnDestroy()
    {
        if (Instance = this)
            Instance = null;

        await Stop();
    }

    private void DebugLogger_LogMessageReceived(object sender, DebugLogMessageEventArgs e)
    {
        var msg = $"{Log.Timestamp()} Discord-{e.Level}: {e.Message}";
        switch (e.Level)
        {
            case LogLevel.Debug:
                Debug.Log(msg);
                break;

            case LogLevel.Info:
                Debug.Log(msg);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(msg);
                break;

            case LogLevel.Error:
                Debug.LogError(msg);
                break;

            case LogLevel.Critical:
                Debug.LogAssertion(msg);
                break;
        }
    }

    private Task Client_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
    {
        Debug.Log($"{Log.Timestamp()} Client is Ready!");
        return Task.CompletedTask;
    }

    private DiscordConfiguration GenerateConfig(string token)
    {
        return new DiscordConfiguration
        {
            Token = token,
            TokenType = TokenType.Bot,

            AutoReconnect = true,
            LogLevel = LogLevel.Debug,
            UseInternalLogHandler = false
        };
    }
}