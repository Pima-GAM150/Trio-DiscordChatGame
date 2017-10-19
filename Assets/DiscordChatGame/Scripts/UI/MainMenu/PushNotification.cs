#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PushNotification : MonoBehaviour
{
    private ScrollRect _scrollRect;
    private bool _queueActive;

    public GameObject PushNotificationPrefab;
    public int Duration;

    private Queue<Tuple<GameObject, int>> _activeQueue = new Queue<Tuple<GameObject, int>>();
    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public GameObject Add(string msg, Color color)
    {
        var obj = Instantiate(PushNotificationPrefab, _scrollRect.content);
        var text = obj.GetComponent<Text>();

        text.color = color;
        text.text = msg;
        _activeQueue.Enqueue(new Tuple<GameObject, int>(obj, Mathf.RoundToInt(Time.time) + Duration));

        if (!_queueActive)
            StartQueue();

        //Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();

        return obj;
    }

    private async Task StartQueue()
    {
        if (_queueActive)
            return;

        _queueActive = true;

        while (_activeQueue.Count > 0)
        {
            var obj = _activeQueue.Dequeue();

            if (Time.time > obj.Item2)
                Destroy(obj.Item1);
            else
            {
                await Task.Delay(Mathf.RoundToInt(obj.Item2 - Time.time));
                Destroy(obj.Item1);
            }
        }
        _queueActive = false;
    }
}