using UnityEngine;

public class AntiCheatLaser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if collided with player
        if (other.gameObject == SplinePlayerController.Instance.gameObject)
        {
            Vector3 direction = transform.forward.normalized * 8f;
            float horizontalForce = new Vector2(direction.x, direction.z).magnitude;
            SplinePlayerController.Instance.SetSpeed(horizontalForce, direction.y * 0.5f);
        }
    }
}
