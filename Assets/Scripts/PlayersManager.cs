using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersManager : Singleton<PlayersManager>
{
    NetworkVariable<int> playersInGame = new NetworkVariable<int>();  // permissions

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    private void Start()
    {
        // who is geeeting 
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if(IsServer)
            {
                Debug.Log($"{id} connected");
                playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"{id} disconnected");
                playersInGame.Value--;
            }
        };
    }
}
