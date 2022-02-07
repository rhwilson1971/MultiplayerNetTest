using Unity.Collections;
using Unity.Netcode;
using TMPro;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();

    bool isPlayerNameSet = false;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            playersName.Value = $"Player {OwnerClientId}";
        }
    }

    public void SetPlayerName()
    {
        var localPlayerName = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerName.text = playersName.Value;
    }

    private void Update()
    {
        if(!isPlayerNameSet && !string.IsNullOrEmpty(playersName.Value))
        {
            SetPlayerName();
            isPlayerNameSet = true;
        }
    }
}