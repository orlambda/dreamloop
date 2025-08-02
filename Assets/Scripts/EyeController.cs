using UnityEngine;
using UnityEngine.Splines;

public class EyeController : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    [SerializeField] private Transform _lookTarget;
    [SerializeField] private float _splineNormalPos;
    [SerializeField] private Vector3 _splinePosOffset;
    private Transform _transform;
    public static EyeController Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _transform = transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lookTarget = SplinePlayerController.Instance.transform;
    }

    private void FixedUpdate()
    {
        if(_spline != null)
        {
            // Chase Player
            _splineNormalPos = SplinePlayerController.Instance.GetSplineNormalPosition();
            // Update position
            _spline.Spline.Evaluate(_splineNormalPos, out var pos, out var dir, out var up);
            Vector3 splineVector = new Vector3(_spline.transform.position.x, 0f, _spline.transform.position.z);
            Vector3 nextPos = splineVector + new Vector3(pos.x, 0f, pos.z) + _splinePosOffset;
            _transform.position = Vector3.Lerp(_transform.position, nextPos, 0.1f);
        }

        Vector3 targetDirection = (_lookTarget.position - _transform.position).normalized;
        _transform.forward = targetDirection;
    }

    public void SetSpline(SplineContainer spline)
    {
        _spline = spline;
    }
}
