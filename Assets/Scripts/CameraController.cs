using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Splines;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private CinemachineSplineDolly _splineDoly;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void EnableDamping()
    {
        _splineDoly.Damping.Enabled = true;
    }
    public void DisableDamping()
    {
        _splineDoly.Damping.Enabled = false;
    }

    public void ResetToStart()
    {
        DisableDamping();
        _splineDoly.CameraPosition = 0f;
    }

    public void SetSpline(SplineContainer spline)
    {
        _splineDoly.Spline = spline;
    }
}