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
    [SerializeField] private TMP_Text _performanceText;
    [SerializeField] private List<string> _performance;
    private Level _currentLevel;
    public static LevelManager Instance;

    [Header("Rest Area")]
    [SerializeField] private Level _restLevel;

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
            _currentLevel.SetLevelState(Level.LEVELSTATE.STARTED);
        }
    }

    private void UpdateTimerText()
    {
        if(_currentLevel.GetLevelState() == Level.LEVELSTATE.STARTED)
        {
            _timerText.text = _currentLevel.GetTimeLeft().ToString("F2");
        }
    }

    public void BeginTheLoop()
    {
        if(_gameState == GAMESTATE.READY)
        {
            _currentLevel.SetLevelTime(30f);
            TransportEye();
            BeginLevel();
            _gameState = GAMESTATE.PLAYING;
        }
    }

    public void GoToNextLevel()
    {
        if(_currentIndex != _finalIndex)
        {
            _currentIndex++;
            _currentLevel = _levelList[_currentIndex];
            if (_currentLevel != null)
            {
                SendPlayerToBeginningOfLevel();
                TransportEye();
                OrchestratedEvents.MusicLordInstance.IncreaseIndex();
            }
        }
    }

    public void SendPlayerToBeginningOfLevel()
    {
        if (_gameState == GAMESTATE.PLAYING)
        {
            // Stop rest level
            _restLevel.SetLevelState(Level.LEVELSTATE.NOTSTARTED);

            Vector3 startingPoint = _currentLevel.GetLevelStartPosition();
            SplineContainer levelSpline = _currentLevel.GetLevelSpline();
            SplinePlayerController.Instance.MultiplySpeed(0.2f);
            SplinePlayerController.Instance.TeleportToPosition(levelSpline, startingPoint);
            CameraController.Instance.SetSpline(levelSpline);
            CameraController.Instance.ResetToStart();
            _currentLevel.ResetLevel();
            BeginLevel();
        }
    }

    public void SendPlayerToRest()
    {
        Vector3 startingPoint = _restLevel.GetLevelStartPosition();
        SplineContainer levelSpline = _restLevel.GetLevelSpline();
        SplinePlayerController.Instance.MultiplySpeed(0.2f);
        SplinePlayerController.Instance.TeleportToPosition(levelSpline, startingPoint);
        CameraController.Instance.SetSpline(levelSpline);
        CameraController.Instance.ResetToStart();

        float timeLeft = _currentLevel.GetTimeLeft();
        _restLevel.SetLevelTime(timeLeft);
        _restLevel.SetLevelState(Level.LEVELSTATE.STARTED);

        // Complete previous level
        if (_currentLevel != null)
        {
            _currentLevel.SetLevelState(Level.LEVELSTATE.COMPLETED);
            _performance.Insert(_currentIndex, _currentLevel.GetTimeElapsed().ToString("F2"));
        }

        // Show score if final level done
        if (_currentIndex == _finalIndex)
        {
            _timerText.text = "Well Done!";
            TallyPerformance();
            _gameState = GAMESTATE.COMPLETED;
        }
        else
        {
            _timerText.text = "";
        }
    }

    public void DoLevelProgreession()
    {
        // Completed level send player to next
        if(_currentLevel != null)
        {
            if(_currentLevel.GetLevelState() == Level.LEVELSTATE.COMPLETED)
            {
                if(_currentIndex != _finalIndex)
                {
                    GoToNextLevel();
                }
            }
            else // Not completed level, restart
            {
                SendPlayerToBeginningOfLevel();
            }
        }
    }

    public void TransportEye()
    {
        if(_currentLevel != null)
        {
            EyeController.Instance.SetSpline(null);
            EyeController.Instance.transform.position = _currentLevel.transform.position + new Vector3(0f, 3.25f, 0f);
            SplineContainer eyeSpline = _currentLevel.GetEyeSpline();
            EyeController.Instance.SetSpline(eyeSpline);
        }
    }

    public void TallyPerformance()
    {
        for(int i=0; i<_performance.Count; i++)
        {
            _performanceText.text += ("LEVEL " + (i+1).ToString() + " TIME: " + _performance[i] + "\n");
        }
    }
}
