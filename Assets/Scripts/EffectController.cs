using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _hitEffect;

    [SerializeField]
    private ParticleSystem _muzzleEffect;
    
    [SerializeField]
    private float _playDuration;
    
    public void PlayHitEffect(Vector3 hitPosition, Vector3 normal)
    {
        var hitEffect =Instantiate(_hitEffect, hitPosition, Quaternion.LookRotation(normal));
        hitEffect.Play();
        
        Destroy(hitEffect.gameObject,_playDuration);
    }

    public void PlayMuzzleEffect(Vector3 position, Vector3 normal)
    {
        var muzzleEffect = Instantiate(_muzzleEffect, position, Quaternion.LookRotation(normal));
        muzzleEffect.Play();
        
        Destroy(muzzleEffect.gameObject, _playDuration);
            
    }
    
}