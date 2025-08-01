using UnityEngine;
using UnityEngine.Events;

public class LevelStarter : MonoBehaviour
{
    public UnityEvent startLevel;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == SplinePlayerController.Instance.gameObject)
        {
            startLevel.Invoke();
            Destroy(gameObject);
        }
    }
}
