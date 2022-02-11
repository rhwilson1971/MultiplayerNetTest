using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using static MultiNetTest;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerBallControls : NetworkBehaviour, IPlayerActions
{
    [SerializeField]
    MultiNetTest playerControls;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    private Rigidbody ballRigidBody;

    private float speed = 0.5f;

    private float flySpeed = 3.5f;

    private Vector3 force;

    public override void OnNetworkSpawn()
    {
        NetworkLog.LogInfoServer($"OK, spawning a client IsOwner={IsOwner} IsClient={IsClient} IsLocalPlayer={IsLocalPlayer}");

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

    private void Update()
    {
        UpdateBall();
    }

    private void Start()
    {
        if (IsOwner && IsClient)
        {
            transform.position =
                new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 1, Random.Range(defaultPositionRange.x, defaultPositionRange.y));
        }
    }

    void IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        NetworkLog.LogInfoServer($"Player Ball Where is this action being handled? {OwnerClientId} local? {IsLocalPlayer} Client? {IsClient} owner? {IsOwner}");

        if (IsOwner && IsClient)
        {
            if (context.performed)
            {
                Vector2 axis = context.ReadValue<Vector2>();

                Debug.Log($"Axis {axis}");

                if(axis.y == -1 || axis.y == 1)
                {
                    Debug.Log($"working on y? {ballRigidBody}");
                    // ballRigidBody.AddForce(axis.y == 1 ? Vector3.forward * speed : Vector3.back * speed);
                    force = axis.y == 1 ? Vector3.forward * speed : Vector3.back * speed;
                }

                if (axis.x == -1 || axis.x == 1)
                {
                    Debug.Log($"working on x? {ballRigidBody}");
                    // ballRigidBody.AddForce(axis.x == 1 ? Vector3.right* speed : Vector3.left * speed);
                    force = axis.x == 1 ? Vector3.right * speed : Vector3.left * speed;
                }
            }
        }
    }

    private void UpdateBall()
    {
        if(IsClient && IsOwner)
            ballRigidBody.AddForce(force);
    }

    void IPlayerActions.OnLook(InputAction.CallbackContext context)
    {
        Vector2 data = context.ReadValue<Vector2>();

        Debug.Log($"Reading value from on Look {data}");
    }

    void IPlayerActions.OnFire(InputAction.CallbackContext context)
    {
        
    }

    private void Awake()
    {
        ballRigidBody = GetComponent<Rigidbody>();
    }
}
