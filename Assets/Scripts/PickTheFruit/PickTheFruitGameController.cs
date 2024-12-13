using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickTheFruitGameController : MonoBehaviour
{
    private const string MainSceneName = "MainScene";

    [SerializeField] private GetReadyPlane _getReadyPlane;
    [SerializeField] private GameOverPlane _gameOverPlane;
    [SerializeField] private PausePlane _pausePlane;
    [SerializeField] private PickTheFruitGameView _view;
    [SerializeField] private PickFruitPlayer _player;
    [SerializeField] private InteractableObjectSpawner _spawner;

    private int _fruitCount;

    public event Action<InteractableObject> FruitCatched;

    private void Start()
    {
        _pausePlane.Disable();
        _gameOverPlane.Disable();
        _spawner.StopSpawning();
        _fruitCount = 0;
        _view.SetFruitValue(_fruitCount.ToString());
        _view.MakeTransperent();
        _getReadyPlane.Enable();
        _player.DisableMovement();
        _getReadyPlane.CountdownComplete += StartNewGame;
    }

    private void OnEnable()
    {
        _player.GoodFruitCatched += ProcessGoodFruitCatched;
        _player.BadFruitCatched += ProcessGameLost;

        _view.PauseClicked += ProcessGamePause;
    }

    private void OnDisable()
    {
        _player.GoodFruitCatched -= ProcessGoodFruitCatched;
        _player.BadFruitCatched -= ProcessGameLost;

        _view.PauseClicked -= ProcessGamePause;

        _gameOverPlane.RestartClicked -= StartNewGame;
        _gameOverPlane.GoToHomeClicked -= GoToMainScreen;

        _pausePlane.ContinueClicked -= ContinueGame;
        _pausePlane.RestartClicked -= StartNewGame;
        _pausePlane.HomeClicked -= GoToMainScreen;
    }
    
    private void StartNewGame()
    {
        _gameOverPlane.Disable();
        _gameOverPlane.RestartClicked -= StartNewGame;
        _gameOverPlane.GoToHomeClicked -= GoToMainScreen;

        _pausePlane.Disable();
        _view.Enable();
        _getReadyPlane.CountdownComplete -= StartNewGame;
        _getReadyPlane.Disable();
        _fruitCount = 0;
        _view.SetFruitValue(_fruitCount.ToString());
        _spawner.EnableSpawn();
        _player.EnableMovement();
    }
    
    private void ProcessGameLost()
    {
        _gameOverPlane.Enable(_fruitCount.ToString());
        _gameOverPlane.RestartClicked += StartNewGame;
        _gameOverPlane.GoToHomeClicked += GoToMainScreen;
        
        _spawner.StopSpawning();
        _player.DisableMovement();
    }

    private void ProcessGoodFruitCatched(InteractableObject fruit)
    {
        if (fruit == null)
            throw new ArgumentNullException(nameof(fruit));

        _fruitCount++;
        _view.SetFruitValue(_fruitCount.ToString());
        FruitCatched?.Invoke(fruit);
    }

    private void GoToMainScreen()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    private void ProcessGamePause()
    {
        _spawner.StopSpawning();
        _player.DisableMovement();

        _view.MakeTransperent();

        _pausePlane.Enable(_fruitCount.ToString());
        _pausePlane.ContinueClicked += ContinueGame;
        _pausePlane.RestartClicked += StartNewGame;
        _pausePlane.HomeClicked += GoToMainScreen;
    }

    private void ContinueGame()
    {
        _spawner.EnableSpawn();
        _player.EnableMovement();

        _pausePlane.ContinueClicked -= ContinueGame;
        _pausePlane.RestartClicked -= StartNewGame;
        _pausePlane.HomeClicked -= GoToMainScreen;

        _pausePlane.Disable();
        _view.Enable();
    }
}