using Unity.Collections;
using Unity.Netcode;

public struct NetworkString : INetworkSerializable
{
    private FixedString32Bytes info;

    void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        serializer.SerializeValue(ref info);
    }

    public override string ToString()
    {
        return info.ToString();
    }

    public static implicit operator string(NetworkString s) => s.ToString();
    public static implicit operator NetworkString(string s) => new NetworkString() { info = new FixedString32Bytes(s) };
}