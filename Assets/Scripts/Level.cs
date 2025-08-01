using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class Level : MonoBehaviour
{
    public UnityEvent timeUp;
    [Header("Level Parameters")]
    [SerializeField] private float _maxLevelTimer;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _timerStarted = false;
    [SerializeField] private Transform _startPos;
    [SerializeField] private SplineContainer _levelSpline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentTime = _maxLevelTimer;
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
        }
    }

    public void BeginCountdown()
    {
        _timerStarted = true;
    }

    public void StopTimer()
    {
        _timerStarted = false;
    }

    public void ResetTimer()
    {
        _currentTime = _maxLevelTimer;
    }

    public float GetTimeLeft()
    {
        return _currentTime;
    }

    public Vector3 GetLevelStartPosition()
    {
        return _startPos.position;
    }

    public SplineContainer GetSplineContainer()
    {
        return _levelSpline;
    }
}