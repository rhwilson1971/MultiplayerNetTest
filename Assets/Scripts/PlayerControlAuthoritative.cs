using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using static MultiNetTest;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerControlAuthoritative : NetworkBehaviour, IPlayerActions
{
    [SerializeField]
    MultiNetTest playerControls;

    [SerializeField]
    private float walkSpeed = 0.005f;

    [SerializeField]
    private bool walking = false;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField]
    float leftRightPosition;

    [SerializeField]
    float forwardBackPosition;

    private void OnEnable()
    {
        NetworkLog.LogInfoServer($"OK, spawning a client IsOwner={IsOwner} IsClient={IsClient} IsLocalPlayer={IsLocalPlayer} IsServer {IsServer}");

        if(IsOwner && IsClient)
        {
            if (playerControls == null)
            {
                playerControls = new MultiNetTest();
                playerControls.Player.SetCallbacks(this);
            }
            playerControls.Player.Enable();
        }
    }

    private void OnDisable()
    {
        if (IsOwner && IsClient)
        {
            playerControls?.Player.Disable();
        }
    }

    private void Update()
    {
        if (walking)
        {
            StartCoroutine(Walk());
            walking = false;
        }
    }

    private IEnumerator Walk()
    {
        float t = Time.time;
        float maxTime = Time.time + 0.5f;

        while (t < maxTime)
        {
            t = Time.time;
            UpdateClientLocation();
            yield return null;
        }
    }

    private void Start()
    {
        if (IsOwner && IsClient)
        {
            transform.position =
                new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 1, Random.Range(defaultPositionRange.x, defaultPositionRange.y));
        }
    }

    void UpdateClientLocation()
    {
        transform.position = new Vector3(transform.position.x + leftRightPosition, transform.position.y, transform.position.z + forwardBackPosition);
    }

    void IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        float forwardBackward = 0;
        float leftRight = 0;

        NetworkLog.LogInfoServer($"Where is this action being handled? {OwnerClientId} local? {IsLocalPlayer} Client? {IsClient} owner? {IsOwner}");

        if (IsOwner && IsClient)
        {
            if (context.performed)
            {
                if (context.control.name == "leftArrow" || context.control.name == "a")
                {
                    leftRight -= walkSpeed;
                }
                if (context.control.name == "upArrow" || context.control.name == "w")
                {
                    forwardBackward += walkSpeed;
                }
                if (context.control.name == "rightArrow" || context.control.name == "d")
                {
                    leftRight += walkSpeed;
                }
                if (context.control.name == "downArrow" || context.control.name == "s")
                {
                    forwardBackward -= walkSpeed;
                }

                leftRightPosition = leftRight;
                forwardBackPosition = forwardBackward;
                walking = true;
            }
        }
    }

    void IPlayerActions.OnLook(InputAction.CallbackContext context)
    {
        
    }

    void IPlayerActions.OnFire(InputAction.CallbackContext context)
    {
        
    }
}
