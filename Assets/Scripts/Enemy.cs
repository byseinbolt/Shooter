using System.Collections;
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
    private Weapon _weapon;
    private WaitForSeconds _delayBeforeDropWeapon;
    private WaitForSeconds _delayBeforeNextShot;
    
    private float _walkSpeed = 2;
    private float _runSpeed = 4;
    private bool _isDead;
    
    
    public void Initialize(Transform target)
    {
        _healthHandler = GetComponent<HealthHandler>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animationController = GetComponent<UnitAnimationController>();
        
        _delayBeforeDropWeapon = new WaitForSeconds(1);
        _delayBeforeNextShot = new WaitForSeconds(2);
        
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

       ChangeWalkMode();
       RotateToTarget(_target);
       MoveToShootingRange(_target);
       
    }
    
    private void ChangeWalkMode()
    {
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            _animationController.SetAimMode(false);
            _navMeshAgent.speed = _runSpeed;
        }
        else
        {
            _navMeshAgent.speed = 0;
            _animationController.SetAimMode(true);
            _weapon.UnitShoot(_target);
        }
        
        _animationController.SetSpeedState(_navMeshAgent.speed);
       
    }
    

    private void MoveToShootingRange(Transform target)
    {
        var distanceToTarget = Vector3.Distance(transform.position, target.position);
        var shootingDistance = Mathf.Max(0, distanceToTarget - _shootingRange);
        var shootingDirection = (target.position - transform.position).normalized;
        var destination = transform.position + shootingDistance * shootingDirection;
        _navMeshAgent.destination = destination;
    }

    private void RotateToTarget(Transform target)
    {
        var aimTarget = target.position;
        aimTarget.y = transform.position.y;
        var aimDirection = (aimTarget - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime*20f);
    }
    private void OnDied()
    {
        _isDead = true;
        _navMeshAgent.enabled = false;
        _animationController.Died();
        StartCoroutine(WaitBeforeDropWeapon());
    }

   

    private void TakeWeapon()
    {
        _weapon = Instantiate(_weaponPrefab, _weaponParent);
    }

    private void DropWeapon()
    {
        var weaponRigidbody = _weapon.AddComponent<Rigidbody>();
        weaponRigidbody.mass = 15;
        weaponRigidbody.drag = 3;
        weaponRigidbody.angularDrag = 3;
        _weapon.transform.SetParent(null, true);
    }

    private IEnumerator WaitBeforeDropWeapon()
    {
        yield return _delayBeforeDropWeapon;
        DropWeapon();
    }
    
    
}