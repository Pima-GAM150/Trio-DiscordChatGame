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

    public bool IsMain()
    {
        return Thread.CurrentThread.ManagedThreadId == _mainThreadID;
    }
    // Executes the next action in the queue if any.
    void Update()
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

    public void Add(System.Action del)
    {
        lock (_queue)
            _queue.Enqueue(del);
    }
}