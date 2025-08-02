using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class Level : MonoBehaviour
{
    public enum LEVELSTATE
    {
        NOTSTARTED,
        STARTED,
        COMPLETED
    }
    public UnityEvent timeUp;
    [Header("Level Parameters")]
    [SerializeField] private LEVELSTATE _currentState = LEVELSTATE.NOTSTARTED;
    [SerializeField] private float _maxLevelTimer;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _timerStarted = false;
    [SerializeField] private Transform _startPos;
    [SerializeField] private SplineContainer _levelSpline;
    [SerializeField] private SplineContainer _eyeSpline;
    [SerializeField] private Transform _orbTransform;
    [SerializeField] private Doorway _levelDoor;
    private Vector3 _orbScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentTime = _maxLevelTimer;
        _orbScale = _orbTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerStarted && _currentTime > 0f)
        {
            _currentTime = Mathf.MoveTowards(_currentTime, 0f, Time.deltaTime);
            if(_currentTime == 0f)
            {
                timeUp.Invoke();
            }
            _orbTransform.localScale = _orbScale * (_currentTime / _maxLevelTimer);
        }
    }

    public void SetLevelState(LEVELSTATE state)
    {
        _currentState = state;

        if(_currentState == LEVELSTATE.NOTSTARTED)
        {

        }
        else if (_currentState == LEVELSTATE.STARTED)
        {
            _timerStarted = true;
        }
        else if (_currentState == LEVELSTATE.COMPLETED)
        {
            _timerStarted = false;
        }
    }

    public void CompleteLevel()
    {
        SetLevelState(LEVELSTATE.COMPLETED);
    }

    public LEVELSTATE GetLevelState()
    {
        return _currentState;
    }

    public void SetLevelTime(float time)
    {
        _maxLevelTimer = time;
        _currentTime = _maxLevelTimer;
    }

    public void ResetLevel()
    {
        _orbTransform.localScale = _orbScale;
        _currentTime = _maxLevelTimer;
        _levelDoor.ResetGate();
    }

    public float GetTimeLeft()
    {
        return _currentTime;
    }

    public float GetTimeElapsed()
    {
        return _maxLevelTimer - _currentTime;
    }

    public Vector3 GetLevelStartPosition()
    {
        return _startPos.position;
    }

    public SplineContainer GetLevelSpline()
    {
        return _levelSpline;
    }

    public SplineContainer GetEyeSpline()
    {
        return _eyeSpline;
    }
}