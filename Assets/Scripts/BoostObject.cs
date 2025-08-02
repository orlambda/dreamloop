using UnityEngine;

public class BoostObject : MonoBehaviour
{
    [SerializeField] private float _respawnTime;
    [SerializeField] private float _rotateSpeed;
    private float _timeToRespawn;
    private GameObject _boostCapsule;
    private Transform _transform;
    private void Awake()
    {
        _boostCapsule = transform.GetChild(0).gameObject;
        _transform = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _boostCapsule.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(_timeToRespawn > 0f)
        {
            _timeToRespawn = Mathf.MoveTowards(_timeToRespawn, 0f, Time.deltaTime);
            if(_timeToRespawn == 0f)
            {
                _boostCapsule.SetActive(true);
            }
        }

        // Rotate
        _transform.Rotate(0f, _rotateSpeed * Time.deltaTime, 0f);
    }

    public void ResetSpawnTimer()
    {
        _timeToRespawn = _respawnTime;
    }
}
