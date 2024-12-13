using System;
using UnityEngine;
using UnityEngine.UI;

public class MemoriPlane : MainMenuGamePlane
{
    [SerializeField] private Sprite _selectedButtonSprite;
    [SerializeField] private Sprite _defaultButtonSprite;
    
    [SerializeField] private Button _2x2Button;
    [SerializeField] private Button _3x4Button;
    [SerializeField] private Button _4x6Button;

    private Button _currenButton;

    private void OnEnable()
    {
        SceneName = "MemoriScene";
        
        MainScreen.MemoriClicked += Enable;
        SubscribeToEvents();
        
        _2x2Button.onClick.AddListener(On2x2ButtonClicked);
        _3x4Button.onClick.AddListener(On3x4ButtonClicked);
        _4x6Button.onClick.AddListener(On4x6ButtonClicked);

        OnDifficultyButtonClicked(_2x2Button);
    }

    private void OnDisable()
    {
        MainScreen.MemoriClicked -= Enable;
        UnsubscribeToEvents();
        
        _2x2Button.onClick.RemoveListener(On2x2ButtonClicked);
        _3x4Button.onClick.RemoveListener(On3x4ButtonClicked);
        _4x6Button.onClick.RemoveListener(On4x6ButtonClicked);
    }
    
    private void On2x2ButtonClicked()
    {
        OnDifficultyButtonClicked(_2x2Button);
    }

    private void On3x4ButtonClicked()
    {
        OnDifficultyButtonClicked(_3x4Button);
    }

    private void On4x6ButtonClicked()
    {
        OnDifficultyButtonClicked(_4x6Button);
    }

    private void OnDifficultyButtonClicked(Button button)
    {
        if (_currenButton != null)
        {
            ResetButton(_currenButton);
        }

        _currenButton = button;
        _currenButton.image.sprite = _selectedButtonSprite;
        SelectDifficulty();
    }

    private void ResetButton(Button button)
    {
        button.image.sprite = _defaultButtonSprite;
    }

    private void SelectDifficulty()
    {
        if (_currenButton == _2x2Button)
        {
            PlayerPrefs.SetInt("Difficulty", 1);
        }
        else if(_currenButton == _3x4Button)
        {
            PlayerPrefs.SetInt("Difficulty", 2);
        }
        else
        {
            PlayerPrefs.SetInt("Difficulty", 3);
        }
    }
}
