using System;
using UnityEngine;
using System.IO;

public interface IConfig
{
    public void Write();

    public CCFConfig Read();
}

public enum CCFMode { Controller, Client, Standalone };

public class CCFConfigSettings : MonoBehaviour, IConfig
{
    public string CCF_Config_File = "CCFConfig.json";

    CCFConfig _configData;
    public CCFConfig CCFConfigData
    {
        get { return _configData;  }
    }

    public CCFConfig Read()
    {
        CCFConfig configData = null;
        string CcfConfigPath = Path.Combine(Application.persistentDataPath, CCF_Config_File);

        try
        {
            if (File.Exists(CcfConfigPath))
            {
                string jsonConfig = File.ReadAllText(CcfConfigPath);
                configData = JsonUtility.FromJson<CCFConfig>(jsonConfig);
            }
        }
        catch (IOException ex)
        {
            
        }

        return configData;
    }

    public void Write ()
    {
        string CcfConfigPath = Path.Combine(Application.persistentDataPath, CCF_Config_File);
        string ccfConfigJson = JsonUtility.ToJson(_configData,true);
        File.WriteAllText(CcfConfigPath, ccfConfigJson);
    }

    private void Awake()
    {
        _configData = Read() ?? new CCFConfig() 
        { 
            connectionSettings = new ConnectionSettings()
            {
                Port = 7778,
                ServerAddress="127.0.0.1",
                ServerListenAddress = "127.0.0.1",
                ConnectionTimeout = 1000,
                ProtocolType = "UnityTransport",
                MaxConnectAttempts = 60,
                MaxPacketQueueSize = 128,
                MaxPayloadSize = 6144,
                MaxSendQueueSize = 98304,
                HeartbeatTimeout = 1000,
                MaxDisconnectTimeout = 30000
            }
        };

        Debug.Log($"My data {_configData.connectionSettings.ServerAddress}");
    }
}

public class CCFConfig
{
    [SerializeField]
    public ConnectionSettings connectionSettings;

    [SerializeField]
    public bool Logging;
}


public class ConnectionSettings
{
    [SerializeField]
    public string ServerAddress;

    [SerializeField]
    public string ServerListenAddress;

    [SerializeField]
    public int Port;

    [SerializeField]
    public string ProtocolType;

    [SerializeField]
    public CCFMode LastCCfMode;

    [SerializeField]
    public string LastSessionName;

    [SerializeField]
    public int MaxPacketQueueSize;

    [SerializeField]
    public int MaxPayloadSize;

    [SerializeField]
    public int MaxSendQueueSize;

    [SerializeField]
    public int HeartbeatTimeout;

    [SerializeField]
    public int ConnectionTimeout;

    [SerializeField]
    public int MaxConnectAttempts;

    [SerializeField]
    public int MaxDisconnectTimeout;
}