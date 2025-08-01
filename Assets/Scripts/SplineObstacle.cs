using UnityEngine;
using UnityEngine.Splines;

public class SplineObstacle : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    [SerializeField] [Range(0, 1)] private float _normalizedPosition;
    [SerializeField] private float _yPosition;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPosition(_normalizedPosition, _yPosition);
    }

    public void SetPosition(float normalizedPos, float yPos)
    {
        _spline.Spline.Evaluate(normalizedPos, out var pos, out var dir, out var up);
        Vector3 splineVector = new Vector3(_spline.transform.position.x, 0f, _spline.transform.position.z);

        Vector3 nextPos = splineVector + new Vector3(pos.x, yPos, pos.z);
        _transform.position = nextPos;
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
        }
    }
}
