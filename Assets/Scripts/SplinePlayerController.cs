using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplinePlayerController : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    private Transform _transform;
    private Rigidbody _rigidbody;

    [Header("Movement Variables")]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _friction;
    [SerializeField] private float _airFriction;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _boostSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _currentSpeed = 0f;
    [SerializeField] private bool _boosting = false;

    // Ground checking
    [Header("Ground Checking")]
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _groundCheckOffset;
    [SerializeField] private bool _grounded = false;
    [SerializeField] private LayerMask _groundLayer;

    // Wall checking
    [Header("Wall Checking")]
    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private float _wallCheckOffset;
    [SerializeField] private bool _touchingWall = false;
    [SerializeField] private LayerMask _wallLayer;

    public static SplinePlayerController Instance;


    // Input variables 
    private float _xInput = 0f;
    private float _yInput = 0;

    private float _splineLength;
    private float _currentPos;

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
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _splineLength = _spline.Spline.GetLength();
    }

    private void Update()
    {
        GetMoveInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.Instance.BeginTheLoop();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _boosting = true;
        }
        else
        {
            _boosting = false;
        }
    }
    private void FixedUpdate()
    {
        CheckForGround();
        CheckForWall();
        MoveAlongSpline();
        Jump();
    }

    private void GetMoveInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");
    }

    private void MoveAlongSpline()
    {
        // Set acceleration
        float accel = _acceleration;
        float fric = _friction;
        if (!_grounded)
        {
            accel = _airAcceleration;
            fric = _airFriction;
        }

        // Set move speed
        if(_xInput != 0f)
        {
            if (!_boosting)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _maxSpeed * _xInput, accel);
            }
            else
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _boostSpeed * _xInput, accel);
            }
        }
        else
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, fric);
        }

        _currentPos = Mathf.Repeat(_currentPos + _currentSpeed * Time.deltaTime, _splineLength);
        float normalizedPos = _currentPos / _splineLength;
        
        _spline.Spline.Evaluate(normalizedPos, out var pos, out var dir, out var up);
        Vector3 splineVector = new Vector3(_spline.transform.position.x, 0f, _spline.transform.position.z);

        Vector3 nextPos = splineVector + new Vector3(pos.x, _rigidbody.position.y, pos.z);

        if(_xInput != 0f)
        {
            if (_currentSpeed > 0f)
            {
                _transform.forward = dir;
            }
            else if (_currentSpeed < 0f)
            {
                _transform.forward = -dir;
            }
        }
        

        Vector3 vel = nextPos - _rigidbody.position;
        _rigidbody.MovePosition(nextPos);
    }

    private void Jump()
    {
        if(_grounded && _yInput > 0f)
        {
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpSpeed, _rigidbody.linearVelocity.z);
        }
    }

    public void TeleportToPosition(SplineContainer splineContainer, Vector3 position)
    {
        _currentPos = 0f;
        _spline = splineContainer;
        _spline.Spline.Evaluate(_currentPos, out var posit, out var dir, out var up);
        Vector3 splineVector = new Vector3(_spline.transform.position.x, 0f, _spline.transform.position.z);
        Vector3 nextPos = splineVector + new Vector3(posit.x, position.y, posit.z);
        _transform.position = nextPos;
        _transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetSpeed(float horizontal, float vertical)
    {
        _currentSpeed = horizontal;
        _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, vertical, _rigidbody.linearVelocity.z);
    }

    public float GetXSpeed()
    {
        return _currentSpeed;
    }

    public float GetYSpeed()
    {
        return _rigidbody.linearVelocity.y;
    }

    public float GetXInput()
    {
        return _xInput;
    }

    public bool GetGrounded()
    {
        return _grounded;
    }

    public void CheckForGround()
    {
        Vector3 groundCheckPos = _transform.position + new Vector3(0f, _groundCheckOffset, 0f);
        _grounded = Physics.CheckSphere(groundCheckPos, _groundCheckRadius, _groundLayer);
    }

    public void CheckForWall()
    {
        Vector3 wallCheckPos = _transform.position + (_transform.forward * _wallCheckOffset);
        _touchingWall = Physics.CheckSphere(wallCheckPos, _wallCheckRadius, _wallLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, _groundCheckOffset, 0f), _groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position + (transform.forward * _wallCheckOffset), _wallCheckRadius);
    }
}