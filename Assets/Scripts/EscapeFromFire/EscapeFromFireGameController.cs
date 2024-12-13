using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeFromFireGameController : MonoBehaviour
{
    private const float MaxTimerValue = 99999;
    private const string MainSceneName = "MainScene";
    
    [SerializeField] private WoodLogSpawner _spawner;
    [SerializeField] private GameOverPlane _gameOverPlane;
    [SerializeField] private PausePlane _pausePlane;
    [SerializeField] private Player _player;
    [SerializeField] private GetReadyPlane _getReadyPlane;
    [SerializeField] private EscapeFromFireGameView _view;
    
    private float _currentTime;
    private IEnumerator _timerCoroutine;
    
    
    private void Start()
    {
        _pausePlane.Disable();
        _gameOverPlane.Disable();
        _spawner.StopSpawning();
        
        _view.MakeTransperent();
        _getReadyPlane.Enable();
        _player.MakeTransperent();
        _getReadyPlane.CountdownComplete += StartNewGame;
    }

    private void OnEnable()
    {
        _player.HitWood += ProcessGameLost;

        _view.PauseClicked += ProcessGamePause;
    }

    private void OnDisable()
    {
        _getReadyPlane.CountdownComplete -= StartNewGame;
        _player.HitWood -= ProcessGameLost;
        _gameOverPlane.RestartClicked -= StartNewGame;
        _gameOverPlane.GoToHomeClicked -= GoToMainScene;
        
        _pausePlane.ContinueClicked -= ContinueGame;
        _pausePlane.RestartClicked -= StartNewGame;
        _pausePlane.HomeClicked -= GoToMainScene;
        
        _view.PauseClicked -= ProcessGamePause;
    }

    private void StartNewGame()
    {
        _gameOverPlane.Disable();
        _pausePlane.Disable();
        _view.Enable();
        _getReadyPlane.CountdownComplete -= StartNewGame;
        _getReadyPlane.Disable();
        _currentTime = 0;
        _spawner.EnableSpawn();
        _player.EnableMovement();

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        
        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);
    }
    
    private IEnumerator StartTimer()
    {
        while (_currentTime < MaxTimerValue)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }
    }
    
    private void UpdateTimer(float time)
    {
        float seconds = Mathf.FloorToInt(time % 60);
        string timeFormatted = seconds.ToString("00000");
        _view.SetTimerValue(timeFormatted);
    }

    private void ProcessGameLost()
    {
        _spawner.StopSpawning();
        _spawner.ReturnAllObjects();
        _player.DisableMovement();
        _gameOverPlane.Enable(_currentTime.ToString("00000"));
        _gameOverPlane.RestartClicked += StartNewGame;
        _gameOverPlane.GoToHomeClicked += GoToMainScene;

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        
        _timerCoroutine = null;
    }

    private void GoToMainScene()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    private void ProcessGamePause()
    {
        _spawner.StopSpawning();
        _spawner.StopAllLogs();
        _player.DisableMovement();

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        
        _view.MakeTransperent();
        _player.MakeTransperent();
        
        _pausePlane.Enable(_currentTime.ToString("00000"));
        _pausePlane.ContinueClicked += ContinueGame;
        _pausePlane.RestartClicked += StartNewGame;
        _pausePlane.HomeClicked += GoToMainScene;
    }

    private void ContinueGame()
    {
        _spawner.EnableSpawn();
        _spawner.EnableAllLogs();
        _player.EnableMovement();

        if (_timerCoroutine != null)
        {
            StartCoroutine(_timerCoroutine);
        }
        
        _pausePlane.ContinueClicked -= ContinueGame;
        _pausePlane.RestartClicked -= StartNewGame;
        _pausePlane.HomeClicked -= GoToMainScene;
        
        _pausePlane.Disable();
        _view.Enable();
    }
}
