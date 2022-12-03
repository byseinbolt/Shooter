using UnityEngine;

[RequireComponent(typeof(HealthHandler))]
public class Enemy : MonoBehaviour
{
    private HealthHandler _healthHandler;

    private void Awake()
    {
        _healthHandler = GetComponent<HealthHandler>();
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
        Destroy(gameObject);
    }
}