using System;
using UnityEditor;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public event Action Died;
    public bool IsAlive => _currentHealth > 0;
    
    [field:SerializeField]
    public float StartHealth { get; private set; }

    private float _currentHealth;

    private void Start()
    {
        _currentHealth = StartHealth;
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