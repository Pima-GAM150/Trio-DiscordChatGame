using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Config
{
    public void Save()
    {
        ConfigManager.Instance.SaveConfigFile(this);
    }
}