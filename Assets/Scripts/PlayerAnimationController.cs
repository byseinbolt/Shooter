using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    public void SetSpeedState(float speed)
    {
        _animator.SetFloat(Speed, speed);
    }

    public void SetAimMode(bool isAiming)
    {
        _animator.SetBool(IsAiming,isAiming);
    }
    
}