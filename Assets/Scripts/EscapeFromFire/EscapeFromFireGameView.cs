using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EscapeFromFireGameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Button _pauseButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action PauseClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _pauseButton.onClick.AddListener(OnPauseClicked);
    }

    private void OnDisable()
    {
        _pauseButton.onClick.RemoveListener(OnPauseClicked);
    }

    public void MakeTransperent()
    {
        _screenVisabilityHandler.SetTransperent();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void SetTimerValue(string value)
    {
        _timeText.text = value;
    }

    private void OnPauseClicked() => PauseClicked?.Invoke();
}
