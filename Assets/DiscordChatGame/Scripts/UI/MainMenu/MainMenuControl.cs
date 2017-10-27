#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using UnityEngine;
using UnityEngine.UI;
using DSharpPlus;
using TMPro;

public class MainMenuControl : MonoBehaviour
{
    public UISwitchGroup Switch;
    public TMP_InputField TokenField;
    private Button _tokenButton;

    public void Start()
    {
        var discordConfig = ConfigManager.Instance.GetConfig<DiscordConfig>();
        if (!string.IsNullOrEmpty(discordConfig.Token))
        {
            TokenField.text = discordConfig.Token;
        }
    }

    public void OnTokenScreenCommit(Button b)
    {
        _tokenButton = b;
        _tokenButton.enabled = false;

        TokenField.DeactivateInputField();

        var ctx = DiscordChatActor.Instance;
        Debug.Log($"{Log.Timestamp()} Sending Token to DiscordLauncher");

        ctx.CreateClient(TokenField.text);
        ctx.Client.Ready += Client_Ready;
        ctx.FailedLogin += Client_FailedLogin;
        ctx.Run(TokenField.text);
    }

    private void Client_FailedLogin(object sender, Exception ex)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_FailedLogin(sender, ex));
            return;
        }
        PushNotification.Instance.Add(ex.Message, PushColor.Failed);
        TokenField.ActivateInputField();
        _tokenButton.enabled = true;
    }

    private Task Client_Ready(ReadyEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_Ready(e));

            return Task.CompletedTask;
        }
        var config = ConfigManager.Instance.GetConfig<DiscordConfig>();
        config.Token = TokenField.text;
        config.Save();

        Switch.SwitchTo("MainMenu");

        // switch menu screen.
        return Task.CompletedTask;
    }
}