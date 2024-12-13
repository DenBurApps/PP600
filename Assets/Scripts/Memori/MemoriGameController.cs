using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoriGameController : MonoBehaviour
{
    private const string DifficultyKey = "Difficulty";
    private const string MainSceneKey = "MainScene";

    [SerializeField] private CellHolder _2x2Cell;
    [SerializeField] private CellHolder _3x4Cell;
    [SerializeField] private CellHolder _4x6Cell;
    [SerializeField] private GameOverPlane _gameOverPlane;
    [SerializeField] private PausePlane _pausePlane;
    [SerializeField] private MemoriGameView _view;
    [SerializeField] private CellSpriteProvider _cellSpriteProvider;

    private float _currentTime;
    private IEnumerator _timerCoroutine;
    private Cell _firstCell;
    private Cell _secondCell;
    private int _cellPairs;
    private CellHolder _currentHolder;
    private CellTypeProvider _cellTypeProvider;

    private void Awake()
    {
        _cellTypeProvider = new CellTypeProvider();
        SelectDifficulty();
    }

    private void OnEnable()
    {
        _view.PauseClicked += PauseGame;

        foreach (var cell in _currentHolder.Cells)
        {
            cell.SetCellSpriteProvider(_cellSpriteProvider);
            cell.Clicked += ProcessCellClicked;
        }
    }

    private void OnDisable()
    {
        foreach (var cell in _currentHolder.Cells)
        {
            cell.Clicked -= ProcessCellClicked;
        }

        _gameOverPlane.RestartClicked -= ProcessNewGameStart;
        _gameOverPlane.GoToHomeClicked -= GoToMainScreen;
        _pausePlane.RestartClicked -= ProcessNewGameStart;
        _pausePlane.HomeClicked -= GoToMainScreen;
        _pausePlane.ContinueClicked -= ContinueGame;
        _view.PauseClicked -= PauseGame;
    }

    private void Start()
    {
        ProcessNewGameStart();
    }

    private void SelectDifficulty()
    {
        if (PlayerPrefs.HasKey(DifficultyKey))
        {
            int difficulty = PlayerPrefs.GetInt(DifficultyKey);

            if (difficulty == 1)
            {
                _2x2Cell.Enable();
                _currentHolder = _2x2Cell;
                _3x4Cell.Disable();
                _4x6Cell.Disable();
            }
            else if (difficulty == 2)
            {
                _2x2Cell.Disable();
                _3x4Cell.Enable();
                _currentHolder = _3x4Cell;
                _4x6Cell.Disable();
            }
            else
            {
                _2x2Cell.Disable();
                _3x4Cell.Disable();
                _4x6Cell.Enable();
                _currentHolder = _4x6Cell;
            }
            
            _cellPairs = _currentHolder.Cells.Count / 2;
        }
    }
    
    private void ProcessNewGameStart()
    {
        _view.Enable();
        ResetDefaultValues();

        _gameOverPlane.Disable();
        _pausePlane.Disable();
        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);
        
        List<CellTypes> cellTypesList =  _cellTypeProvider.GetPair(_cellPairs);

        for (int i = 0; i < cellTypesList.Count; i++)
        {
            _currentHolder.Cells[i].ReturnToDefault();
            _currentHolder.Cells[i].SetCellType(cellTypesList[i]);
        }
    }

    private void ProcessCellClicked(Cell cell)
    {
        if (cell.IsFliped)
            return;

        if (_firstCell != null && _secondCell != null)
        {
            _firstCell.HideCellImage();
            _secondCell.HideCellImage();

            _firstCell = null;
            _secondCell = null;
        }

        if (_firstCell == null)
        {
            _firstCell = cell;
            _firstCell.ShowCellImage();
            return;
        }

        if (_secondCell == null && _firstCell != cell)
        {
            _secondCell = cell;
            _secondCell.ShowCellImage();
        }

        if (_firstCell != null && _secondCell != null)
            CompareChosenCells();
    }

    private void CompareChosenCells()
    {
        if (_firstCell.CurrentType == _secondCell.CurrentType)
        {
            _cellPairs--;

            _firstCell.Disable();
            _secondCell.Disable();

            if (_cellPairs <= 0)
            {
                ProcessGameWon();
            }

            _firstCell = null;
            _secondCell = null;
        }
    }

    private void ResetDefaultValues()
    {
        _cellPairs = _currentHolder.Cells.Count / 2;
        _currentTime = 0;
        _view.SetTimeValue(_currentTime.ToString("00000"));

        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        _timerCoroutine = null;

        _firstCell = null;
        _secondCell = null;
    }

    private IEnumerator StartTimer()
    {
        _currentTime = 0;

        while (_currentTime >= 0f)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }
    }

    private void UpdateTimer(float time)
    {
        time = Mathf.Max(time, 0);

        float seconds = Mathf.FloorToInt(time % 60);
        _view.SetTimeValue(seconds.ToString("00000"));
    }

    private void ProcessGameWon()
    {
        _gameOverPlane.Enable(_currentTime.ToString("00000"));
        _gameOverPlane.RestartClicked += ProcessNewGameStart;
        _gameOverPlane.GoToHomeClicked += GoToMainScreen;

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    private void GoToMainScreen()
    {
        SceneManager.LoadScene(MainSceneKey);
    }

    private void PauseGame()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        _view.MakeTransperent();
        _pausePlane.Enable(_currentTime.ToString("00000"));
        _pausePlane.RestartClicked += ProcessNewGameStart;
        _pausePlane.HomeClicked += GoToMainScreen;
        _pausePlane.ContinueClicked += ContinueGame;
    }

    private void ContinueGame()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);
        _view.Enable();
    }
}