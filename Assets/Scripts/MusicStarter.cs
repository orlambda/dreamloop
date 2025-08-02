using UnityEngine;
using UnityEngine.Events;

public class MusicStarter : MonoBehaviour
{
    public UnityEvent startMusic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == SplinePlayerController.Instance.gameObject)
        {
            startMusic.Invoke();
            Destroy(gameObject);
        }
    }
}
