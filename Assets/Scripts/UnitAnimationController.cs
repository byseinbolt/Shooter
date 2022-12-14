using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");
    private static readonly int Speed = Animator.StringToHash("Speed");
    

    public void SetSpeedState(float speed)
    {
        _animator.SetFloat(Speed, speed);
    }
    public void SetAimMode(bool isAiming)
    {
        _animator.SetBool(IsAiming,isAiming);
    }

    public void Died()
    {
        _animator.enabled = false;
    }

   

}