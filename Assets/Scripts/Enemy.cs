using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitAnimationController))]
[RequireComponent(typeof(HealthHandler))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _shootingRange;
    
    [SerializeField]
    private Weapon _weaponPrefab;

    [SerializeField]
    private Transform _weaponParent;
    
    private HealthHandler _healthHandler;
    private NavMeshAgent _navMeshAgent;
    private UnitAnimationController _animationController;
    
    private Transform _target;
    private bool _isDead;
    private Weapon _weapon;
    private float _walkingSpeed = 2;

    private void Awake()
    {
        _healthHandler = GetComponent<HealthHandler>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animationController = GetComponent<UnitAnimationController>();
    }

    public void Initialize(Transform target)
    {
        TakeWeapon();
        _healthHandler.Died += OnDied;
        _target = target;
    }
    
    private void OnDestroy()
    {
        _healthHandler.Died -= OnDied;
    }

    private void Update()
    {
       if (_isDead)
       {
           return;
       }

       if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
       {
           _animationController.SetSpeedState(_walkingSpeed);
           _navMeshAgent.speed = _walkingSpeed;
       }
       else
       {
           _animationController.SetSpeedState(0);
           _navMeshAgent.speed = 0;
       }
       
       MoveToShootingRange(_target);
    }

    private void MoveToShootingRange(Transform target)
    {
        var distanceToTarget = Vector3.Distance(transform.position, target.position);
        var shootingDistance = Mathf.Max(0, distanceToTarget - _shootingRange);
        var shootingDirection = (target.position - transform.position).normalized;
        var destination = transform.position + shootingDistance * shootingDirection;
        _navMeshAgent.destination = destination;
    }
    private void OnDied()
    {
        _isDead = true;
        _navMeshAgent.enabled = false;
        _animationController.Died();
        DropWeapon();
    }

    private void TakeWeapon()
    {
        _weapon = Instantiate(_weaponPrefab, _weaponParent);
    }

    private void DropWeapon()
    {
        _weapon.AddComponent<Rigidbody>();
        _weapon.transform.SetParent(null, true);
    }
    
}