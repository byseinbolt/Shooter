using System.Collections;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int DieTrigger = Animator.StringToHash("DieTrigger");
    private static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int FreeFall = Animator.StringToHash("FreeFall");
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    private readonly WaitForSeconds _deathAnimationPlayTime = new(3f);
    
    public void SetSpeedState(float speed)
    {
        _animator.SetFloat(Speed, speed);
    }

    public void SetMotionSpeedState(float motionSpeed)
    {
        _animator.SetFloat(MotionSpeed, motionSpeed);
    }

    public void SetJumpState(bool isJumping)
    {
        _animator.SetBool(Jump, isJumping);
    }

    public void SetFreeFallState(bool isFreeFall)
    {
        _animator.SetBool(FreeFall, isFreeFall);
    }

    public void SetGroundedState(bool isGrounded)
    {
        _animator.SetBool(Grounded, isGrounded);
    }
    public void SetAimMode(bool isAiming)
    {
        _animator.SetBool(IsAiming,isAiming);
    }

    public void Died()
    {
        _animator.SetFloat(Die, Random.value);
        _animator.SetTrigger(DieTrigger);
        StartCoroutine(WaitTillAnimationPlayed());
    }

    private IEnumerator WaitTillAnimationPlayed()
    {
        yield return _deathAnimationPlayTime;
        _animator.enabled = false;
    }
}