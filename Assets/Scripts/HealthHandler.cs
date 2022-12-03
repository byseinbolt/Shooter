using System;
using UnityEditor;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public event Action Died;
    public bool IsAlive => _currentHealth > 0;
    
    [SerializeField]
    private float _startHealth;

    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _startHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive)
        {
            return;
        }
        _currentHealth -= damage;
        
        if (_currentHealth<=0)
        {
            Died?.Invoke();
        }
    }
}