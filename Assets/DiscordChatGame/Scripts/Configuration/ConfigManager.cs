using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Singleton for getting Configuration Files.
/// </summary>
public class ConfigManager : MonoBehaviour
{
    /// <summary>
    /// Discord Config File.
    /// </summary>
    public DiscordConfig DiscordConfig
    {
        get
        {
            if (_discordConfig == null)
            {
                return LoadConfigFile<DiscordConfig>("DiscordConfig.json");
            }
            return _discordConfig;
        }
        private set
        {
            _discordConfig = value;
        }
    }

    public static ConfigManager ActiveManager;

    private DiscordConfig _discordConfig;

    private string _configPath;

    /// <summary>
    /// Loads a non MonoBehaviour Json Config file.
    /// </summary>
    /// <typeparam name="T">Type of the object to create</typeparam>
    /// <param name="fileName">file name, i.e: DiscordConfig.json</param>
    public T LoadConfigFile<T>(string fileName) where T : new()
    {
        FileInfo file = new FileInfo(_configPath + fileName);
        if (file.Exists)
        {
            using (StreamReader reader = new StreamReader(file.OpenRead()))
            {
                var json = reader.ReadToEnd();
                return JsonUtility.FromJson<T>(json);
            }
        }
        else
        {
            var obj = new T();
            SaveConfigFile(fileName, obj);
            return obj;
        }
    }

    /// <summary>
    /// Loads a MonoBehaviour type Config file.
    /// </summary>
    /// <typeparam name="T">Type of the MonoBehaviour to create</typeparam>
    /// <param name="fileName">file name, i.e: MyMonoBehaviourConfig.json</param>
    /// <param name="obj">the MonoBehaviour object to overwrite.</param>
    public void LoadMonoConfigFile(string fileName, MonoBehaviour obj)
    {
        FileInfo file = new FileInfo(_configPath + fileName);
        if (file.Exists)
        {
            using (StreamReader reader = new StreamReader(file.OpenRead()))
            {
                var json = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(json, obj);
            }
        }
        else
        {
            SaveConfigFile(fileName, obj);
        }
    }

    /// <summary>
    /// Saves an object to a config file with the specified name.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="obj"></param>
    public void SaveConfigFile(string fileName, object obj)
    {
        FileInfo file = new FileInfo(_configPath + fileName);

        FileStream stream;

        if (file.Exists)
        {
            stream = file.OpenWrite();
        }
        else
        {
            stream = file.Create();
        }

        using (StreamWriter writer = new StreamWriter(stream))
        {
            var json = JsonUtility.ToJson(obj);
            writer.Write(json);
        }
    }

    public void Awake()
    {
        if (ActiveManager == null)
        {
            ActiveManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning($"{LogHelpers.FormatTimestamp()} Destroying {this} on {name}. Multiple {this} found.");
            Destroy(this);
        }
    }

    public void Start()
    {
        _configPath = Application.persistentDataPath + "/";
    }
}