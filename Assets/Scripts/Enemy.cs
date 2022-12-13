using UnityEngine;


[RequireComponent(typeof(HealthHandler))]
public class Enemy : MonoBehaviour
{
    private HealthHandler _healthHandler;
    private Animator _animator;

    private void Awake()
    {
        _healthHandler = GetComponent<HealthHandler>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _healthHandler.Died += OnDied;
    }

    private void OnDestroy()
    {
        _healthHandler.Died -= OnDied;
    }

    private void OnDied()
    {
        _animator.enabled = false;
    }
}