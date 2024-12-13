using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPlane : MonoBehaviour
{
    [SerializeField] private Button _goToHomeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMP_Text _time;

    public event Action GoToHomeClicked;
    public event Action RestartClicked;

    private void OnEnable()
    {
        _goToHomeButton.onClick.AddListener(GoToHomeButtonClicked);
        _restartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnDisable()
    {
        _goToHomeButton.onClick.RemoveListener(GoToHomeButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartClicked);
    }

    public void Enable(string time)
    {
        gameObject.SetActive(true);
        _time.text = time;
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void GoToHomeButtonClicked() => GoToHomeClicked?.Invoke();
    private void OnRestartClicked() => RestartClicked?.Invoke();
}
