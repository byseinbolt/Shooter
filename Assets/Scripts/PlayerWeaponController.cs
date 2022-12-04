using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;

    private PlayerInput _playerInput;
    private float _timeAfterPreviousShot;
    private RaycastHit _hitTarget;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.Enable();
        _playerInput.Player.Shoot.performed += OnShoot;
    }

    private void OnDestroy()
    {
        _playerInput.Player.Shoot.performed -= OnShoot;
        _playerInput.Player.Disable();
    }


    public void Initialize(RaycastHit hitTarget)
    {
        _hitTarget = hitTarget;
    }
    
    private void OnShoot(InputAction.CallbackContext obj)
    {
       _weapon.Shoot(_hitTarget);
    }
}