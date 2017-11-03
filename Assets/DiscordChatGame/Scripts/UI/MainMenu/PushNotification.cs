#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum PushColor
{
    Debug,
    Info,
    Warning,
    Error,
    Success,
    Failed
}

public class PushNotification : MonoBehaviour
{
    public static PushNotification Instance { get; private set; }

    private ScrollRect _scrollRect;

    [Tooltip("Color to use on push notifications from Discord-Info")]
    public Color ColorInfo = Color.white;

    [Tooltip("Color to use on push notifications from Discord-Error")]
    public Color ColorError = Color.red;

    [Tooltip("Color to use on push notifications from Discord-Warning")]
    public Color ColorWarning = Color.yellow;

    [Tooltip("Color to use on push notifications from Discord-Debug")]
    public Color ColorDebug = Color.grey;

    [Tooltip("Color to use for positive push notifications")]
    public Color ColorSuccess = Color.green;

    [Tooltip("Color to use for negative push notifications")]
    public Color ColorFailed = Color.red;

    public GameObject PushNotificationPrefab;
    public int Duration = 2;

    private void Awake()
    {
        if (transform.parent.GetComponent<Canvas>())
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform.parent);
            }
            else
                Destroy(this);
        }
    }

    private Color GetColor(PushColor color)
    {
        switch (color)
        {
            case PushColor.Debug:
                return ColorDebug;

            case PushColor.Info:
                return ColorInfo;

            case PushColor.Warning:
                return ColorWarning;

            case PushColor.Error:
                return ColorError;

            case PushColor.Success:
                return ColorSuccess;

            case PushColor.Failed:
                return ColorFailed;

            default:
                return Color.white;
        }
    }

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public GameObject Add(string msg, PushColor color)
    {
        return Add(msg, GetColor(color));
    }

    public GameObject Add(string msg, Color color)
    {
        var obj = Instantiate(PushNotificationPrefab, _scrollRect.content);
        var text = obj.GetComponent<Text>();

        text.color = color;
        text.text = msg;

        QueueDestroy(obj, Duration);

        //Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();

        return obj;
    }

    private async Task QueueDestroy(GameObject obj, int destroyTime)
    {
        await Task.Delay(destroyTime * 1000);

        if (Application.isPlaying)
            Destroy(obj);
    }
}