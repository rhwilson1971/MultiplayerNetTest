using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    NetworkVariable<NetworkString> message = new NetworkVariable<NetworkString>();
    NetworkVariable<ulong> theClientId = new NetworkVariable<ulong>();

    Dictionary<ulong, List<string>> chatMessages = new Dictionary<ulong, List<string>>();

    public void SendChat(string chatMessage)
    {
        NetworkLog.LogInfoServer($"Sending chat message {chatMessage} IsClient={IsClient} IsServer{IsServer} IsOwner={IsOwner} gameObject.name {gameObject.name}");
        if (IsClient)
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            UpdateMessageServerRpc(id, chatMessage);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateMessageServerRpc(ulong clientId, string chatMessage)
    {
        Debug.Log($"Server received {message.Value}");
        message.Value = chatMessage;
        theClientId.Value = clientId;

        if (chatMessages.ContainsKey(clientId))
        {
            Debug.Log($"Server received {message.Value}");
        }
        else
        {
            chatMessages.Add(clientId, new List<string>() { chatMessage });
        }

        string chatGuiMessage = $"[{clientId}]: {chatMessage}";
        UpdateClientListClientRpc(chatGuiMessage);
    }

    [ClientRpc]
    void UpdateClientListClientRpc(string message)
    {
        Debug.Log($"Client got a message {message}");
        GameObject.Find("MainViewDocument").GetComponent<UIManager>().OnNewChatMessage(message);
    }
}
