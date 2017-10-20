#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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

    /// <summary>
    /// root object for the main menu page.
    /// </summary>
    public GameObject MainPage;

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

    public void OnTokenScreenCommit(Button b)
    {
        b.enabled = false;
        _tokenField.DeactivateInputField();
        var ctx = DiscordChatActor.Instance;
        Debug.Log($"{Log.Timestamp()} Sending Token to DiscordLauncher");

        ctx.CreateClient(_tokenField.text);
        ctx.Client.Ready += Client_Ready;
        ctx.Client.ClientErrored += Client_ClientErrored;
        ctx.Run();
    }

    private Task Client_ClientErrored(ClientErrorEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_ClientErrored(e));

            return Task.CompletedTask;
        }
        _tokenField.ActivateInputField();
        return Task.CompletedTask;
    }

    private Task Client_Ready(ReadyEventArgs e)
    {
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(() => Client_Ready(e));

            return Task.CompletedTask;
        }

        // switch menu screen.
        TokenScreen.SetActive(false);
        MainPage.SetActive(true);
        return Task.CompletedTask;
    }
}