using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ChatClient : INetworkSerializable
{
    private FixedString128Bytes info;
    ulong clientId;

    void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        serializer.SerializeValue(ref info);
        serializer.SerializeValue(ref clientId);
    }


}
