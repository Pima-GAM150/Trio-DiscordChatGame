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
    /// collection of Config files.
    /// </summary>
    private Dictionary<Type, Config> _configs = new Dictionary<Type, Config>();

    public static ConfigManager Instance;

    private DiscordConfig _discordConfig;

    private string _configPath;

    /// <summary>
    /// Gets the active Config loaded for this type or loads a config file and returns it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetConfig<T>() where T : Config, new()
    {
        if (_configs.ContainsKey(typeof(T)))
        {
            return (T)_configs[typeof(T)];
        }
        else
        {
            return LoadConfigFile<T>();
        }
    }

    /// <summary>
    /// Loads a non MonoBehaviour Json Config file.
    /// </summary>
    /// <typeparam name="T">Type of the object to create</typeparam>
    public T LoadConfigFile<T>() where T : Config, new()
    {
        FileInfo file = new FileInfo(_configPath + typeof(T) + ".json");
        Config cfg;
        if (file.Exists)
        {
            using (StreamReader reader = new StreamReader(file.OpenRead()))
            {
                var json = reader.ReadToEnd();
                Debug.Log($"{Log.ShortTime()} Loading Config from {file.FullName}");
                cfg = JsonUtility.FromJson<T>(json);
            }
        }
        else
        {
            cfg = new T();
            Debug.Log($"{Log.ShortTime()} Created Config {file.FullName}");
            SaveConfigFile(cfg);
        }
        _configs.Add(typeof(T), cfg);

        return (T)cfg;
    }

    /// <summary>
    /// Loads a MonoBehaviour type Config file.
    /// </summary>
    /// <typeparam name="T">Type of the MonoBehaviour to create</typeparam>
    /// <param name="obj">the MonoBehaviour object to overwrite.</param>
    public void LoadMonoConfigFile(MonoBehaviour obj)
    {
        FileInfo file = new FileInfo(_configPath + obj.GetType() + ".json");
        if (file.Exists)
        {
            using (StreamReader reader = new StreamReader(file.OpenRead()))
            {
                var json = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(json, obj);
            }
        }
    }

    /// <summary>
    /// Saves an object to a config file with the specified name.
    /// </summary>
    /// <param name="obj">object to save</param>
    public void SaveConfigFile(object obj)
    {
        FileInfo file = new FileInfo(_configPath + obj.GetType() + ".json");

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
        Debug.Log($"{Log.ShortTime()} Saved Config file {file.FullName}");
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning($"{Log.Timestamp()} Destroying {this} on {name}. Multiple {this} found.");
            Destroy(this);
        }
    }

    public void Start()
    {
        _configPath = Application.persistentDataPath + "/";
    }
}