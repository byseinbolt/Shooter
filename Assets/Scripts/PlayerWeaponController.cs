using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;
    
    private RaycastHit _hitTarget;
    
    public void Initialize(RaycastHit hitTarget)
    {
        _hitTarget = hitTarget;
    }
    
    public void OnShoot(InputAction.CallbackContext obj)
    {
       _weapon.Shoot(_hitTarget);
    }
}