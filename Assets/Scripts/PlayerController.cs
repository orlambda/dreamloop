using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerController Instance;
    private Transform _myTransform;
    private Rigidbody _myRigidbody;

    // Movement parameters
    [Header("Movement Variables")]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _friction;
    [SerializeField] private float _airFriction;
    private float _horizontalDir = 1f;
    private float _xInput = 0f;
    private float _yInput = 0;
    private bool _jump = false;
    [SerializeField] private float _currentSpeed = 0f;
    [SerializeField] private float _jumpForce;

    // Ground checking
    [Header("Ground Checking")]
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _groundCheckOffset;
    [SerializeField] private bool _grounded = false;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Flight")]
    [SerializeField] private JetWings _jetWings;
    [SerializeField] private float _rotateAccel;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _currentRotateSpeed;
    [SerializeField] private float _currentAngle;
    private float _zRotation = 0f;

    [Header("Boositng")]
    [SerializeField] private float _boostAcceleration;
    [SerializeField] private float _boostSpeed;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }

        _myTransform = transform;
        _myRigidbody = GetComponent<Rigidbody>();
        _currentAngle = _myRigidbody.rotation.eulerAngles.x;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveInput();
    }

    private void FixedUpdate()
    {
        DoMovement();
        DoJump();
        DoRotation();
        CheckForGround();
    }

    private void GetMoveInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");


        if (!_grounded)
        {
            if(_yInput == 0f)
            {
                _myRigidbody.useGravity = true;
            }
            else
            {
                _myRigidbody.useGravity = false;
            }
        }
        else
        {
            if(_jump == false && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                _jump = true;
            }
        }

        _horizontalDir = Mathf.Sign(Vector3.Dot(_myTransform.forward, Vector3.forward));
    }

    private void DoMovement()
    {
        if (_yInput > 0f)
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _boostSpeed, _acceleration);
            _myRigidbody.linearVelocity = Vector3.MoveTowards(_myRigidbody.linearVelocity, _myTransform.forward * _currentSpeed, _boostAcceleration);
        }
        else
        {
            if (_grounded)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _friction);
            }
            else
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _airFriction);
            }
            
            _myRigidbody.linearVelocity = new Vector3(_myRigidbody.linearVelocity.x, _myRigidbody.linearVelocity.y, _myRigidbody.linearVelocity.z*0.95f);
        }
    }

    private void DoJump()
    {
        if (_jump)
        {
            _myRigidbody.linearVelocity = new Vector3(_myRigidbody.linearVelocity.x, _jumpForce, _myRigidbody.linearVelocity.z);
            _jump = false;
        }
    }

    private void DoRotation()
    {
        if (_xInput != 0f)
        {
            if (!_grounded)
            {
                if (_yInput > 0f)
                {
                    _currentRotateSpeed = Mathf.MoveTowards(_currentRotateSpeed, _xInput * _rotateSpeed * 0.5f, _rotateAccel);
                    // Change z rotation while flying
                    if (_horizontalDir > 0)
                    {
                        _zRotation = 0;
                    }
                    else
                    {
                        _zRotation = 180f;
                    }
                }
                else
                {
                    _currentRotateSpeed = Mathf.MoveTowards(_currentRotateSpeed, _xInput * _rotateSpeed, _rotateAccel);
                }
            }
            else
            {
                _currentRotateSpeed = 0f;
            }
        }
        else
        {
            _currentRotateSpeed = Mathf.MoveTowards(_currentRotateSpeed, 0f, _rotateAccel * 0.5f);
        }
        Vector3 rotateVel = new Vector3(_currentRotateSpeed, 0f, 0f);
        _currentAngle += _currentRotateSpeed;
        _myRigidbody.rotation = Quaternion.Euler(_currentAngle + _currentRotateSpeed, 0f, _zRotation);
    }

    public void CheckForGround()
    {
        Vector3 groundCheckPos = _myTransform.position + new Vector3(0f, _groundCheckOffset, 0f);
        _grounded = Physics.CheckSphere(groundCheckPos, _groundCheckRadius, _groundLayer);
        if (_grounded)
        {
            _jetWings.OpenWings(false);
            _currentRotateSpeed = 0f;
            
            if(_horizontalDir > 0)
            {
                _currentAngle = 0;
                _zRotation = 0f;
                _myRigidbody.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                _currentAngle = 180;
                _zRotation = 180f;
                _myRigidbody.rotation = Quaternion.Euler(_currentAngle, 0f, 180f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, _groundCheckOffset, 0f), _groundCheckRadius);
    }
}
