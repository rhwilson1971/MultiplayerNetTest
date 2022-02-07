using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkChatManager : NetworkBehaviour
{
    [SerializeField]
    NetworkVariable<int> currentCamera = new NetworkVariable<int>(NetworkVariableReadPermission.OwnerOnly,1);
    // NetworkVariable<int> switchCamera = new NetworkVariable<int>(NetworkVariableReadPermission.OwnerOnly, 1);

    public NetworkVariable<int> CurrentCamera => currentCamera;


    private void OnEnable()
    {
        CurrentCamera.OnValueChanged += (oldValue, newValue) => 
        { 
            if(IsOwner && IsClient)
            {
                // switch the camera
            }
        };
    }

    void OnCameraSwitched(int oldValue, int newValue)
    {

    }

    private void OnDisable()
    {
        CurrentCamera.OnValueChanged -= OnCameraSwitched;    
    }
}
