using UnityEngine;
using UnityEngine.Splines;

public class SplineObstacle : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    [SerializeField] [Range(0, 1)] private float _normalizedPosition;
    [SerializeField] private float _yPosition;
    [SerializeField] private bool _isMoving;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _autoSetPosition = false;
    [SerializeField] private bool _followSplineY = false;
    private float _positionLerp = 0;
    private Vector3 _spawnPosition;
    private Transform _transform;
    private Vector3 _baseScale;

    private void Awake()
    {
        _transform = transform;
        _baseScale = transform.localScale;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spawnPosition = _transform.position;
        if (_autoSetPosition)
        {
            _positionLerp = 1f;
            SetPosition(_normalizedPosition, _yPosition);
        }
    }

    public void SetPosition(float normalizedPos, float yPos)
    {
        _spline.Spline.Evaluate(normalizedPos, out var pos, out var dir, out var up);
        Vector3 splineVector = new Vector3(_spline.transform.position.x, 0f, _spline.transform.position.z);

        float yPosition = yPos;
        if (_followSplineY)
        {
            yPosition = pos.y;
        }

        Vector3 nextPos = splineVector + new Vector3(pos.x, yPosition, pos.z);
        _transform.position = Vector3.Lerp(_spawnPosition, nextPos, _positionLerp);
        _transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided with player
        if(other.gameObject == SplinePlayerController.Instance.gameObject)
        {
            Vector3 direction = (other.transform.position - transform.position).normalized * 15f;
            float dotProduct = Vector3.Dot(direction, transform.forward);
            float horizontalForce = new Vector2(direction.x, direction.z).magnitude;
            SplinePlayerController.Instance.SetSpeed(Mathf.Sign(dotProduct)*horizontalForce, direction.y*0.5f);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        FluctuateOrb();

        if (_isMoving)
        {
            _normalizedPosition = Mathf.Repeat(_normalizedPosition + _moveSpeed * Time.deltaTime, 1f);
            SetPosition(_normalizedPosition, _yPosition);
        }
    }

    private void FluctuateOrb()
    {
        float scaleFactor = Random.Range(0.9f, 1.2f);
        Vector3 _newScale = _baseScale * scaleFactor;
        _transform.localScale = _newScale;
    }
}
