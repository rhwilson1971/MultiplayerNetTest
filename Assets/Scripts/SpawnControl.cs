using Unity.Netcode;
using UnityEngine;
using System;

public class SpawnControl : Singleton<SpawnControl>
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject objectPrefab;

    [SerializeField]
    private int maxObjectInstanceCount = 3;

    private void Awake()
    {

    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("called once?");
            NetworkObjectPool.Instance.InitializePool();
        };
    }

    public void SpawnObjects()
    {
        if (!IsServer)
        {
            NetworkLog.LogInfoServer("This is not a server");
            return;
        }

        NetworkLog.LogInfoServer("Attempting to initialize and spawn an object");

        for (int i = 0; i < maxObjectInstanceCount; i++)
        {
            //GameObject go = Instantiate(objectPrefab, new Vector3(Random.Range(-10, 10), 10.0f, Random.Range(-10, 10)), Quaternion.identity);
            //go.GetComponent<NetworkObject>().Spawn();

            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(objectPrefab).gameObject;
            go.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), 10.0f, UnityEngine.Random.Range(-10, 10));
            go.GetComponent<NetworkObject>().Spawn();

        }
    }
}
