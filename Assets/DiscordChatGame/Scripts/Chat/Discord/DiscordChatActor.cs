#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void

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
    public delegate void LogMessage(string message, LogLevel level);

    public event LogMessage OnLogMessage;

    public static DiscordChatActor Instance { get; private set; }
    public DiscordClient Client { get; private set; }

    public async Task Run()
    {
        if (Client == null)
            throw new NullReferenceException("Discord client was not created before calling Run. Use CreateClient(token) before!");

        await Client.ConnectAsync();
    }

    public async Task Stop()
    {
        await Client.DisconnectAsync();
    }

    public void CreateClient(string token)
    {
        // Disable SSL Certificate Validation
        // see: https://dsharpplus.emzi0767.com/articles/hosting_rpi.html#method-4-run-your-bot-using-mono
        ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;

        // Setup Client only if this is the first time.
        if (Client == null)
        {
            Client = new DiscordClient(GenerateConfig(token));
            Client.SetWebSocketClient<WebSocketSharpClient>();

            Client.Ready += Client_Ready;
            Client.DebugLogger.LogMessageReceived += DebugLogger_LogMessageReceived;
            Client.ClientErrored += Client_ClientErrored;
        }
    }

    private Task Client_ClientErrored(ClientErrorEventArgs e)
    {
        Debug.LogError($"{Log.Timestamp()} Discord-ClientErrored: {e.Exception}");
        OnLogMessage?.Invoke("Error while connecting!", LogLevel.Error);
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
                OnLogMessage?.Invoke(e.Message, e.Level);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(msg);
                OnLogMessage?.Invoke(e.Message, e.Level);
                break;

            case LogLevel.Error:
                Debug.LogError(msg);
                OnLogMessage?.Invoke(e.Message, e.Level);
                break;

            case LogLevel.Critical:
                Debug.LogAssertion(msg);
                OnLogMessage?.Invoke(e.Message, e.Level);
                break;
        }
    }

    private Task Client_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
    {
        Debug.Log($"{Log.Timestamp()} Discord Client is Ready! Username: {e.Client.CurrentUser.Username}");
        OnLogMessage?.Invoke($"Client Connected Successfully as {e.Client.CurrentUser.Username}", LogLevel.Info);
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