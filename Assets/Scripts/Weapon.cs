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

    private float _currentTime;
    private float _delayBeforeNextShot = 1f;
    
    private void Awake()
    {
        _effectController = GetComponent<EffectController>();
    }
    
    public void PlayerShoot(RaycastHit targetHit)
    {
        _effectController.PlayMuzzleEffect(_muzzle.transform.position, _muzzle.forward);
        
        var aimDirection = (targetHit.point - _muzzle.position).normalized;
        var bullet = Instantiate(_bullet, _muzzle.position, Quaternion.LookRotation(aimDirection, Vector3.up));
         bullet.Initialize(targetHit.point);
         
        var otherEffectController = targetHit.transform.GetComponentInParent<EffectController>();
        if (otherEffectController != null)
        {
            otherEffectController.PlayHitEffect(targetHit.point, targetHit.normal);
        }

        var otherHealthHandler = targetHit.transform.GetComponentInParent<HealthHandler>();
        if (otherHealthHandler != null)
        {
            if (targetHit.collider.gameObject.CompareTag("UnitHead"))
            {
                otherHealthHandler.TakeDamage(otherHealthHandler.StartHealth);
            }
        
            otherHealthHandler.TakeDamage(_damage);
        }
    }

    public void UnitShoot(Transform target)
    {
        _currentTime += Time.deltaTime;
        
        if (_currentTime>_delayBeforeNextShot)
        {
            _effectController.PlayMuzzleEffect(_muzzle.position, _muzzle.forward);
            
            var aimDirection = (target.position - _muzzle.position).normalized;
            var bullet = Instantiate(_bullet, _muzzle.position, Quaternion.LookRotation(aimDirection, Vector3.up));
            bullet.Initialize(target.position);
        
            var otherEffectController = target.gameObject.GetComponent<EffectController>();
            if (otherEffectController != null)
            {
                otherEffectController.PlayHitEffect(target.position, target.position);
            }

            var otherHealthHandler = target.gameObject.GetComponent<HealthHandler>();
            if (otherHealthHandler != null)
            {
                otherHealthHandler.TakeDamage(_damage);
            }

            _currentTime = 0;
        }
    }
}