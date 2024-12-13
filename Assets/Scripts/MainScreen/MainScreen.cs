using System;
using Unity.VisualScripting;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private MainScreenView _view;
    [SerializeField] private EscapeFromFirePlane _escapeFromFirePlane;
    [SerializeField] private PickTheFruitPlane _pickTheFruitPlane;
    [SerializeField] private MemoriPlane _memoriPlane;
    [SerializeField] private SettingsScreen _settingsScreen;

    public event Action SettingsClicked;
    public event Action EscapeFromFireClicked;
    public event Action PickTheFruitClicked;
    public event Action MemoriClicked;
    
    private void Start()
    {
        _view.Enable();
    }

    private void OnEnable()
    {
        _view.SettingButtonClicked += SettingsOpened;
        _view.EscapeFromFireClicked += EscapeFromFireOpened;
        _view.PickTheFruitClicked += OnPickTheFruitClicked;
        _view.MemoriClicked += OnMemoryClicked;
        _settingsScreen.BackButtonClicked += _view.Enable;

        _escapeFromFirePlane.BackButtonClicked += _view.Enable;
        _pickTheFruitPlane.BackButtonClicked += _view.Enable;
        _memoriPlane.BackButtonClicked += _view.Enable;
    }

    private void OnDisable()
    {
        _view.SettingButtonClicked -= SettingsOpened;
        _view.EscapeFromFireClicked -= EscapeFromFireOpened;
        _view.PickTheFruitClicked -= OnPickTheFruitClicked;
        _view.MemoriClicked -= OnMemoryClicked;
        _settingsScreen.BackButtonClicked -= _view.Enable;
        
        _escapeFromFirePlane.BackButtonClicked -= _view.Enable;
        _pickTheFruitPlane.BackButtonClicked -= _view.Enable;
        _memoriPlane.BackButtonClicked -= _view.Enable;
    }

    private void SettingsOpened()
    {
        SettingsClicked?.Invoke();
        _view.Disable();
    }

    private void EscapeFromFireOpened()
    {
        EscapeFromFireClicked?.Invoke();
        _view.Disable();
    }

    private void OnPickTheFruitClicked()
    {
        PickTheFruitClicked?.Invoke();
        _view.Disable();
    }

    private void OnMemoryClicked()
    {
        MemoriClicked?.Invoke();
        _view.Disable();
    }
}
