using UnityEngine;
using UnityEngine.Events;

public class CameraTrigger : MonoBehaviour
{
    public UnityEvent resetCameraDamping;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == SplinePlayerController.Instance.gameObject)
        {
            resetCameraDamping.Invoke();
        }
    }
}
