﻿#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void

using DSharpPlus;
using DSharpPlus.Net.WebSocket;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using DSharpPlus.EventArgs;
using System.Net;
using DSharpPlus.Exceptions;
using DSharpPlus.Entities;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;

public class DiscordChatActor : MonoBehaviour
{
    public DiscordChannel InformationChannel;
    public DiscordChannel SpawnEnemiesChannel;
    public DiscordChannel IncomeFeedChannel;
    public DiscordChannel CheckBalanceChannel;
    public DiscordGuild Guild;
    public CommandsNextExtension Commands;
    public List<DiscordMember> Members = new List<DiscordMember>();

    public delegate void ExceptionEventHandler(object sender, Exception ex);

    public static DiscordChatActor Instance { get; private set; }
    public DiscordClient Client { get; private set; }

    public event ExceptionEventHandler FailedLogin;

    public async Task Run(string token)
    {
        if (Client == null)
            CreateClient(token);
        try
        {
            await Client.ConnectAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"{Log.Timestamp()} Discord-Login: {e.Message}");
            FailedLogin?.Invoke(Client, e);
            Client = null;
        }
    }

    public async Task Stop()
    {
        if (Client != null)
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

            var ccfg = new CommandsNextConfiguration
            {
                // let's use the string prefix defined in config.json
                StringPrefix = "!",

                // enable responding in direct messages
                EnableDms = true,

                // enable mentioning the bot as a command prefix
                EnableMentionPrefix = true
            };

            Commands = Client.UseCommandsNext(ccfg);
            Commands.RegisterCommands<DiscordCommands>();

            Client.DebugLogger.LogMessageReceived += DebugLogger_LogMessageReceived;
            Client.Ready += Client_Ready;
            Client.GuildAvailable += Client_GuildAvailable;
            Client.GuildMemberAdded += Client_GuildMemberAdded;
            Client.GuildMemberRemoved += Client_GuildMemberRemoved;
            Client.ClientErrored += Client_ClientErrored;
        }
    }

    private async Task Client_GuildAvailable(GuildCreateEventArgs e)
    {
        await SetupGuildDefaults(e.Guild);
    }

    private Task Client_GuildMemberRemoved(GuildMemberRemoveEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_GuildMemberRemoved(e));
            return Task.CompletedTask;
        }

        if (e.Guild == Guild && Members.Contains(e.Member))
            Members.Remove(e.Member);

        return Task.CompletedTask;
    }

    private Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_GuildMemberAdded(e));
            return Task.CompletedTask;
        }
        if (e.Guild == Guild)
        {
            Members.Add(e.Member);
        }
        return Task.CompletedTask;
    }

    private async Task Client_Ready(ReadyEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(async () => await Client_Ready(e));
            return;
        }
        Debug.Log($"{Log.Timestamp()} Discord-ClientReady: Client is connected.");
        PushNotification.Instance.Add($"Connected as: {e.Client.CurrentUser.Username}", PushColor.Success);
        foreach (var connectedGuild in Client.Guilds)
        {
            try
            {
                if (connectedGuild.Value.GetRole(Client.CurrentUser.Id).CheckPermission(Permissions.ManageGuild) == PermissionLevel.Allowed)
                    await connectedGuild.Value.DeleteAsync();
            }
            catch { }
        }

        await Task.Delay(1000);

        if (Guild == null)
        {
            var guild = await Client.CreateGuildAsync(Client.CurrentUser.Username + UnityEngine.Random.Range(100, 999));
            await SetupGuildDefaults(guild);
        }
    }

    private async Task SetupGuildDefaults(DiscordGuild guild)
    {
        if (Guild == null)
        {
            Guild = guild;
            try
            {
                InformationChannel = await guild.CreateChannelAsync("Information", ChannelType.Text);
                SpawnEnemiesChannel = await guild.CreateChannelAsync("SpawnEnemies", ChannelType.Text);
                IncomeFeedChannel = await guild.CreateChannelAsync("IncomeFeed", ChannelType.Text);
                CheckBalanceChannel = await guild.CreateChannelAsync("CheckBalance", ChannelType.Text);
            }
            catch { }
            var invite = await InformationChannel.CreateInviteAsync();
            Debug.Log($"{Log.Timestamp()} Join: https://discord.gg/{invite.Code}");
            PushNotification.Instance.Add($"Join: https://discord.gg/{invite.Code}", PushColor.Success);
        }
    }

    private Task Client_ClientErrored(ClientErrorEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_ClientErrored(e));
            return Task.CompletedTask;
        }

        Debug.LogError($"{Log.Timestamp()} Discord-ClientErrored: {e.Exception}");
        PushNotification.Instance.Add("Failed to connect to discord. Is your key valid?", PushColor.Failed);
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
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => DebugLogger_LogMessageReceived(sender, e));
            return;
        }

        var msg = $"{Log.Timestamp()} Discord-{e.Level}: {e.Message}";
        var push = PushNotification.Instance;
        switch (e.Level)
        {
            case LogLevel.Debug:
                Debug.Log(msg);
                push.Add(e.Message, PushColor.Debug);
                break;

            case LogLevel.Info:
                Debug.Log(msg);
                push.Add(e.Message, PushColor.Info);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(msg);
                push.Add(e.Message, PushColor.Warning);
                break;

            case LogLevel.Error:
                Debug.LogError(msg);
                push.Add(e.Message, PushColor.Error);
                break;

            case LogLevel.Critical:
                Debug.LogAssertion(msg);
                push.Add(e.Message, PushColor.Error);
                break;
        }
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