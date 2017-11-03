using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISwitchGroup : MonoBehaviour
{
    public string InitialSwitchableName = "";

    private Dictionary<string, GameObject> _switchables = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (Transform obj in transform)
        {
            Debug.Log($"{obj.name}");
            _switchables.Add(obj.name, obj.gameObject);
        }
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(InitialSwitchableName))
        {
            if (_switchables.Count > 0)
            {
                var firstKey = _switchables.Keys.First();
                SwitchTo(firstKey);
            }
        }
        else
            SwitchTo(InitialSwitchableName);
    }

    public void AddSwitchable(string name, GameObject switchable)
    {
        if (_switchables.ContainsKey(name))
            return;

        _switchables.Add(name, switchable);
    }

    public void RemoveSwitchable(string name)
    {
        if (!_switchables.ContainsKey(name))
            return;

        _switchables.Remove(name);
    }

    public void SwitchTo(string name)
    {
        foreach (var obj in _switchables)
        {
            if (obj.Key == name)
                obj.Value.SetActive(true);
            else
                obj.Value.SetActive(false);
        }
    }
}