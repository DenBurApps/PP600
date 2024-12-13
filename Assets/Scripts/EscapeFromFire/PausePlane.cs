using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePlane : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continiueButton;
    [SerializeField] private Button _goToHomeButton;
    [SerializeField] private TMP_Text _time;

    public event Action RestartClicked;
    public event Action ContinueClicked;
    public event Action HomeClicked;
    
    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _goToHomeButton.onClick.AddListener(OnHomeClicked);
        _continiueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _goToHomeButton.onClick.RemoveListener(OnHomeClicked);
        _continiueButton.onClick.RemoveListener(OnContinueClicked);
    }

    public void Enable(string text)
    {
        gameObject.SetActive(true);
        _time.text = text;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnRestartClicked() => RestartClicked?.Invoke();
    private void OnContinueClicked() => ContinueClicked?.Invoke();
    private void OnHomeClicked() => HomeClicked?.Invoke();
}
