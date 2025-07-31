using UnityEngine;

public class JetWings : MonoBehaviour
{
    enum WINGSTATE
    {
        OPEN,
        CLOSE
    }
    private Transform _transform;
    [SerializeField] private Transform _leftWing;
    [SerializeField] private Transform _rightWing;
    [SerializeField] private WINGSTATE _state = WINGSTATE.CLOSE;
    private float _wingAngle = 0f;
    private float _leftWingAngle = 0f;
    private float _rightWingAngle = 0f;
    private float _packAngle = 0f;
    [SerializeField] private Material _rightMaterial;
    [SerializeField] private Material _leftMaterial;

    private void Awake()
    {
        _transform = transform;
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rightMaterial = _rightWing.GetChild(0).GetComponent<Renderer>().material;
        _leftMaterial = _leftWing.GetChild(0).GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimateWings();
    }

    public void ToggleWingState()
    {
        if (_state == WINGSTATE.OPEN)
        {
            _state = WINGSTATE.CLOSE;
            _wingAngle = 0f;
            EnableEmission(false);
        }
        else
        {
            _state = WINGSTATE.OPEN;
            _wingAngle = 60f;
        }
    }

    public void OpenWings(bool openWings)
    {
        if (openWings)
        {
            _state = WINGSTATE.OPEN;
            _wingAngle = 60f;
        }
        else
        {
            _state = WINGSTATE.CLOSE;
            _wingAngle = 0f;
            EnableEmission(false);
        }
    }

    public void EnableEmission(bool shouldEnable)
    {
        if (shouldEnable)
        {
            _rightMaterial.EnableKeyword("_EMISSION");
            _leftMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            _rightMaterial.DisableKeyword("_EMISSION");
            _leftMaterial.DisableKeyword("_EMISSION");
        }
    }
    private void AnimateWings()
    {
        if (_leftWing.localEulerAngles.z != -_wingAngle)
        {
            _leftWingAngle = Mathf.MoveTowards(_leftWingAngle, -_wingAngle, 5f);
        }

        if (_rightWing.localEulerAngles.z != _wingAngle)
        {
            _rightWingAngle = Mathf.MoveTowards(_rightWingAngle, _wingAngle, 5f);
        }

        if(_transform.localEulerAngles.x != _wingAngle)
        {
            _packAngle = Mathf.MoveTowards(_packAngle, _wingAngle, 10f);
        }

        _rightWing.localEulerAngles = new Vector3(_rightWing.localEulerAngles.x, _rightWing.localEulerAngles.y, _rightWingAngle);
        _leftWing.localEulerAngles = new Vector3(_leftWing.localEulerAngles.x, _leftWing.localEulerAngles.y, _leftWingAngle);
        _transform.localEulerAngles = new Vector3(_packAngle, _transform.localEulerAngles.y, _transform.localEulerAngles.z);
    }
}