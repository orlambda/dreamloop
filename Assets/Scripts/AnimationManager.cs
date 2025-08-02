using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float xInput = SplinePlayerController.Instance.GetXInput();
        float xSpeed = SplinePlayerController.Instance.GetXSpeed();
        float ySpeed = SplinePlayerController.Instance.GetYSpeed();
        bool grounded = SplinePlayerController.Instance.GetGrounded();
        _animator.SetFloat("XSpeed", Mathf.Abs(xSpeed));
        _animator.SetFloat("YSpeed", ySpeed);
        _animator.SetBool("Grounded", grounded);
        
        //Set skidding
        if(Mathf.Sign(xSpeed) != Mathf.Sign(xInput))
        {
            _animator.SetBool("Skid", true);
        }
        else
        {
            _animator.SetBool("Skid", false);
        }
    }
}
