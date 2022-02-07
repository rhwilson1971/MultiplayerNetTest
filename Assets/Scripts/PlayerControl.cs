using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Samples;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class PlayerControl : NetworkBehaviour
{

    float localA;
    float localB;

    [SerializeField]
    private float walkSpeed = 0.005f;

    [SerializeField]
    private bool walking = false;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    float leftRightPosition;
    float forwardBackPosition;

    public override void OnNetworkSpawn()
    {
        //if(IsClient)
        //{
        //    TestServerRpc(89);
        //}
    }


    public void Nav(InputAction.CallbackContext context)
    {
        float forwardBackward = 0;
        float leftRight = 0;

        Debug.Log($"getting input ? Islocalplayer? {IsLocalPlayer}");
        if (IsLocalPlayer)
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
}
