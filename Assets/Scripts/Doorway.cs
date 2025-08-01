using UnityEngine;
using UnityEngine.Events;

public class Doorway : MonoBehaviour
{
    public UnityEvent passedGate;
    [SerializeField] private GameObject _enteredGameObject;
    private void OnTriggerEnter(Collider other)
    {
        // If entered from behind
        if (Vector3.Dot(other.transform.forward, transform.forward) > 0f)
        {
            _enteredGameObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If exit from front
        if (Vector3.Dot(other.transform.forward, transform.forward) > 0f)
        {
            if(other.gameObject == _enteredGameObject)
            {
                passedGate.Invoke();
            }
            else
            {
                _enteredGameObject = null;
            }
        }
        else
        {
            _enteredGameObject = null;
        }
    }
}
