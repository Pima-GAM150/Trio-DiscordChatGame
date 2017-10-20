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
    /// <summary>
    /// root object for the Discord Token screen.
    /// </summary>
    public GameObject TokenScreen;

    /// <summary>
    /// root object for the main menu page.
    /// </summary>
    public GameObject MainPage;

    private TMP_InputField _tokenField;
    private Button _tokenButton;

    public void Start()
    {
        _tokenField = TokenScreen.GetComponentInChildren<TMP_InputField>();
        if (_tokenField == null)
        {
            Debug.LogError($"{Log.Timestamp()} No InputField on TokenScreen!");
        }
        var discordConfig = ConfigManager.Instance.GetConfig<DiscordConfig>();
        if (!string.IsNullOrEmpty(discordConfig.Token))
        {
            _tokenField.text = discordConfig.Token;
        }
    }

    public void OnTokenScreenCommit(Button b)
    {
        _tokenButton = b;
        _tokenButton.interactable = false;

        _tokenField.DeactivateInputField();

        var ctx = DiscordChatActor.Instance;
        Debug.Log($"{Log.Timestamp()} Sending Token to DiscordLauncher");

        ctx.CreateClient(_tokenField.text);
        ctx.Client.Ready += Client_Ready;
        ctx.FailedLogin += Client_FailedLogin;
        ctx.Run(_tokenField.text);
    }

    private void Client_FailedLogin(object sender, Exception ex)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_FailedLogin(sender, ex));
            return;
        }
        PushNotification.Instance.Add(ex.Message, PushColor.Failed);
        _tokenField.ActivateInputField();
        _tokenButton.interactable = true;
    }

    private Task Client_Ready(ReadyEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_Ready(e));

            return Task.CompletedTask;
        }
        var config = ConfigManager.Instance.GetConfig<DiscordConfig>();
        config.Token = _tokenField.text;
        config.Save();

        // switch menu screen.
        TokenScreen.SetActive(false);
        MainPage.SetActive(true);
        return Task.CompletedTask;
    }
}