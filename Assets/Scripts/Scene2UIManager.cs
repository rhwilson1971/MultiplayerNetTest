using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Scene2UIManager : NetworkBehaviour
{
    [SerializeField]
    private Button previousSceneButton;

    [SerializeField]
    private Button animateModelButton;

    [SerializeField]
    private Button disconnectButton;

    private Animator animator;

    private bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        var rootVisuaElement = GetComponent<UIDocument>().rootVisualElement;

        previousSceneButton = rootVisuaElement.Q<Button>("previous-scene-button");
        animateModelButton = rootVisuaElement.Q<Button>("animate-model-button");
        disconnectButton = rootVisuaElement.Q<Button>("disconnect-button");

        previousSceneButton.RegisterCallback<ClickEvent>((ce) =>
        {
            NetworkLog.LogInfoServer("Loading previous scene");
            NetworkManager.Singleton.SceneManager.LoadScene("Scene1withClientNetworkTransform", UnityEngine.SceneManagement.LoadSceneMode.Single);
        });

        animateModelButton.RegisterCallback<ClickEvent>((ce) =>
        {
            NetworkLog.LogInfoServer("Animating the missile model");
            GameObject missile = GameObject.Find("missile");
            animator = missile.GetComponent<Animator>();

            isAnimating = !isAnimating;

            animator.SetBool("isAnimating", isAnimating);
        });

        disconnectButton.RegisterCallback<ClickEvent>((ce) =>
        {
            // NetworkManager.Singleton.Shutdown();
        });

        if(IsClient && !IsHost)
        {
            previousSceneButton.SetEnabled(false);
            animateModelButton.SetEnabled(false);
        }
    }
}
