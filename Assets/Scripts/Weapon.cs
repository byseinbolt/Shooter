using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field:SerializeField]
    public float DelayBeforeNextShot { get; private set; }
    
    [SerializeField]
    private Bullet _bullet;

    [SerializeField]
    private Transform _muzzle;

    [SerializeField]
    private float _damage;
    
    private EffectController _effectController;
    
    private void Awake()
    {
        _effectController = GetComponent<EffectController>();
    }
    

    public void Shoot(RaycastHit targetHit)
    {
        var aimDirection = (targetHit.point - _muzzle.position).normalized;
        var bullet = Instantiate(_bullet, _muzzle.position, Quaternion.LookRotation(aimDirection, Vector3.up));
         bullet.Initialize(targetHit.point);
         
        _effectController.PlayMuzzleEffect(_muzzle.position, _muzzle.forward);
        
        var otherEffectController = targetHit.transform.GetComponentInParent<EffectController>();
        if (otherEffectController != null)
        {
            otherEffectController.PlayHitEffect(targetHit.point, targetHit.normal);
        }

        var otherHealthHandler = targetHit.transform.GetComponentInParent<HealthHandler>();
        if (otherHealthHandler != null)
        {
            otherHealthHandler.TakeDamage(_damage);
        }
    }

   
}