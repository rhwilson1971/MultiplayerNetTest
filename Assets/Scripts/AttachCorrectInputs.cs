using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttachCorrectInputs : NetworkBehaviour
{
    private PlayerInput playerInput;
    // Assign correct InputActionAsset in the player's prefab inspector
    [SerializeField] private InputActionAsset inputActionAsset;

    private void Start()
    {
        // playerInput = gameObject.GetComponent<PlayerInput>();
    }

    //public override void OnNetworkSpawn()
    //{
    //    base.OnNetworkSpawn();

    //    if (!IsOwner) return;

    //    if(playerInput.actions != inputActionAsset)
    //    {
    //        playerInput.actions = inputActionAsset;
    //    }
    //}

}
