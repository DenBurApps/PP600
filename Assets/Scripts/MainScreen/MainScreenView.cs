using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _escapeFromFireButton;
    [SerializeField] private Button _pickTheFruitButton;
    [SerializeField] private Button _memoriButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action SettingButtonClicked;
    public event Action EscapeFromFireClicked;
    public event Action PickTheFruitClicked;
    public event Action MemoriClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _escapeFromFireButton.onClick.AddListener(OnEscapeFromFireClicked);
        _pickTheFruitButton.onClick.AddListener(OnPickTheFruitClicked);
        _memoriButton.onClick.AddListener(OnMemoriClicked);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _escapeFromFireButton.onClick.RemoveListener(OnEscapeFromFireClicked);
        _pickTheFruitButton.onClick.RemoveListener(OnPickTheFruitClicked);
        _memoriButton.onClick.RemoveListener(OnMemoriClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnSettingsButtonClicked() => SettingButtonClicked?.Invoke();
    private void OnEscapeFromFireClicked() => EscapeFromFireClicked?.Invoke();
    private void OnPickTheFruitClicked() => PickTheFruitClicked?.Invoke();
    private void OnMemoriClicked() => MemoriClicked?.Invoke();
}