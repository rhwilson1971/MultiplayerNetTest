using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button executePhysicsButton;

    [SerializeField]
    private Label playerText;

    [SerializeField]
    private ListView messageList;

    [SerializeField]
    private Button sendMessageButton;

    [SerializeField]
    private TextField messageToSend;

    [SerializeField]
    private Button nextSceneButton;

    private bool hasServerStarted;

    List<string> chatMessages = new List<string>();

    private void Start()
    {
        var rootVisuaElement = GetComponent<UIDocument>().rootVisualElement;

        startServerButton    = rootVisuaElement.Q<Button>("server-button");
        startClientButton    = rootVisuaElement.Q<Button>("client-button");
        startHostButton      = rootVisuaElement.Q<Button>("host-button");
        executePhysicsButton = rootVisuaElement.Q<Button>("execute-physics-button");

        playerText = rootVisuaElement.Q<Label>("player-info-label");

        messageList = rootVisuaElement.Q<ListView>("messages-list");
        messageToSend = rootVisuaElement.Q<TextField>("message-to-send");
        sendMessageButton = rootVisuaElement.Q<Button>("send-button");

        nextSceneButton = rootVisuaElement.Q<Button>("next-scene-button");

        // Create a new label in list to show the data
        Func<VisualElement> makeItem = () => new Label();
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = chatMessages[i];

        messageList.makeItem = makeItem;
        messageList.bindItem = bindItem;
        messageList.fixedItemHeight = 16;
        messageList.itemsSource = chatMessages;

        nextSceneButton.RegisterCallback<ClickEvent>(OnNextScene);

        sendMessageButton.RegisterCallback<ClickEvent>( ev => {
            string myMessage = messageToSend.text;

            // OnNewChatMessage(myMessage);

            if (!string.IsNullOrEmpty(myMessage) )
            {
                NetworkLog.LogInfoServer($"Sending chat message {myMessage}");
                GameObject chatManager = GameObject.Find("ChatManager");
                chatManager.GetComponent<ChatManager>().SendChat(myMessage);
            }
        });
            
        startServerButton.RegisterCallback<ClickEvent>(ev =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Debug.Log("Started started");
            }
            else
            {
                Debug.Log("Server encountered problem");
            }
        });

        startClientButton.RegisterCallback<ClickEvent>(ev =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client started");
            }
            else
            {
                Debug.Log("Client encountered problem");
            }
        });

        startHostButton.RegisterCallback<ClickEvent>(ev =>
        {
            // NetworkManager.Singleton.ConnectionApprovalCallback += OnApproveClient;
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host started");
            }
            else
            {
                Debug.Log("Host encounted problem");
            }
        });

        executePhysicsButton.RegisterCallback<ClickEvent>(ev =>
        {
            if (hasServerStarted)
            {
                Debug.Log("server has not been started");
                return;
            }
            SpawnControl.Instance.SpawnObjects();
        });

        // NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("Did the server message for start occur in UI Manager?");
            hasServerStarted = true;
        };
    }


    public void OnApproveClient(byte[] arg1, ulong arg2, NetworkManager.ConnectionApprovedDelegate callback) 
    {
        bool approve = true;
        bool createPlayerObject = true;

        //ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("MyPrefabHashGenerator");
        Debug.Log("ON approved client");
    }

    private void Update()
    {
        playerText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    public void OnNewChatMessage(string message)
    {
        chatMessages.Add(message);
        messageList.RefreshItems();
    }

    void OnNextScene(ClickEvent ev)
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Scene2", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

