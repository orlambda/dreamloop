using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Splines;

public class LevelManager : MonoBehaviour
{
    private enum GAMESTATE
    {
        READY,
        PLAYING,
        COMPLETED
    }
    [SerializeField] private GAMESTATE _gameState = GAMESTATE.READY;
    [SerializeField] private List<Level> _levelList;
    [SerializeField] private int _currentIndex = 0;
    [SerializeField] private int _finalIndex = 0;
    [SerializeField] private TMP_Text _timerText;
    private Level _currentLevel;
    public static LevelManager Instance;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _finalIndex = _levelList.Count - 1;
        _currentLevel = _levelList[_currentIndex];
    }

    private void FixedUpdate()
    {
        if(_gameState == GAMESTATE.PLAYING)
        {
            UpdateTimerText();
        }
    }

    public void BeginLevel()
    {
        if(_currentLevel != null)
        {
            _timerText.text = _currentLevel.GetTimeLeft().ToString();
            _currentLevel.BeginCountdown();
        }
    }

    private void UpdateTimerText()
    {
        _timerText.text = _currentLevel.GetTimeLeft().ToString("F2");
    }

    public void BeginTheLoop()
    {
        if(_gameState == GAMESTATE.READY)
        {
            BeginLevel();
            _gameState = GAMESTATE.PLAYING;
        }
    }

    public void GoToNextLevel()
    {
        if (_currentLevel != null)
        {
            _currentLevel.StopTimer();
        }
            
        if(_currentIndex == _finalIndex)
        {
            _timerText.text = "Nice " + _currentLevel.GetTimeLeft().ToString("F2");
            _gameState = GAMESTATE.COMPLETED;
        }
    }

    public void SendPlayerToBeginningOfLevel()
    {
        Vector3 startingPoint = _currentLevel.GetLevelStartPosition();
        SplineContainer levelSpline = _currentLevel.GetSplineContainer();
        SplinePlayerController.Instance.TeleportToPosition(levelSpline, startingPoint);
        _currentLevel.ResetTimer();
    }
}
