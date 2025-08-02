using UnityEngine;
using UnityEngine.Events;

public class BoostApplicator : MonoBehaviour
{
    public UnityEvent doEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == SplinePlayerController.Instance.gameObject)
        {
            SplinePlayerController.Instance.AddBoost();
            doEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
