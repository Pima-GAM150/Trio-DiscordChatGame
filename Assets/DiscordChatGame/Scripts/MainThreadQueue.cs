using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainThreadQueue : MonoBehaviour
{
    private readonly Queue<System.Action> _queue = new Queue<System.Action>();
    private int _mainThreadID;
    public static MainThreadQueue Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            _mainThreadID = Thread.CurrentThread.ManagedThreadId;
        }
        else
            Destroy(this);
    }

    /// <summary>
    /// Returns true if called in the main thread.
    /// </summary>
    public bool IsMain()
    {
        return Thread.CurrentThread.ManagedThreadId == _mainThreadID;
    }

    // Executes the next action in the queue if any.
    private void Update()
    {
        if (_queue.Count > 0)
            lock (_queue)
            {
                while (_queue.Count > 0)
                {
                    _queue.Dequeue().Invoke();
                }
            }
    }

    /// <summary>
    /// Adds an action to the queue which will be completed next frame.
    /// </summary>
    public void Queue(System.Action action)
    {
        lock (_queue)
            _queue.Enqueue(action);
    }

    // use this snippet at the top of a method to re-call it on the main thread. will be delayed by a single frame.
    //if (!MainThreadQueue.Instance.IsMain())
    //{
    //MainThreadQueue.Instance.Queue(() => privateClient_Ready(e));

    //return Task.CompletedTask;
    //}
}