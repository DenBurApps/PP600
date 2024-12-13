using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainMenuGamePlane : MonoBehaviour
{
    [SerializeField] protected MainScreen MainScreen;

    protected string SceneName;
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _backButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;


    public event Action BackButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        Disable();
    }

    protected void SubscribeToEvents()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    protected void UnsubscribeToEvents()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    protected void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    protected void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(SceneName);
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        Disable();
    }
}