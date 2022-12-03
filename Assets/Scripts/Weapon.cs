using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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
        
        _effectController.PlayMuzzleEffect(_muzzle.position, _muzzle.forward);
        bullet.Initialize(targetHit.point);
        

        var otherEffectController = targetHit.transform.GetComponent<EffectController>();
        if (otherEffectController != null)
        {
            otherEffectController.PlayHitEffect(targetHit.point, targetHit.normal);
        }

        var otherHealthHandler = targetHit.transform.GetComponent<HealthHandler>();
        if (otherHealthHandler != null)
        {
            otherHealthHandler.TakeDamage(_damage);
        }

    }
}