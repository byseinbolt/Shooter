using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerCameraController))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(HealthHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;
    
    private PlayerAnimationController _animationController;
    private PlayerMovementController _movementController;
    private PlayerCameraController _cameraController;
    private HealthHandler _healthHandler;
    private PlayerInput _input;
    private Camera _camera;

    
    private void Awake()
    {
        _camera = Camera.main;
        _animationController = GetComponent<PlayerAnimationController>();
        _movementController = GetComponent<PlayerMovementController>();
        _cameraController = GetComponent<PlayerCameraController>();
        _healthHandler = GetComponent<HealthHandler>();
        _input = new PlayerInput();
    }

    private void Start()
    {
        _input.Player.Aim.performed += OnAimPerformed;
        _input.Player.Aim.canceled += OnAimCanceled;
        _input.Player.Shoot.performed += OnShootPerformed; 
        _movementController.Stayed += OnStayed;
        _movementController.Moved += OnMoved;
        _input.Player.Enable();
    }
    
    private void OnDestroy()
    {
        _input.Player.Aim.performed -= OnAimPerformed;
        _input.Player.Aim.canceled -= OnAimCanceled;
        _input.Player.Shoot.performed -= OnShootPerformed; 
        _movementController.Stayed -= OnStayed;
        _movementController.Moved -= OnMoved;
        _input.Player.Disable();
    }



    private void Update()
    {
        var moveInput = _input.Player.Move.ReadValue<Vector2>();
        _movementController.Move(moveInput, _camera);
        
        if (_cameraController.IsAimMode)
        {
            _cameraController.PlayerAimRotation(_camera);
            _movementController.SetRotateOnMove(false);
            
        }
        else
        {
            _movementController.SetRotateOnMove(true);
        }
    }

    private void LateUpdate()
    {
        var lookInput = _input.Player.Look.ReadValue<Vector2>();
        _cameraController.CameraRotation(lookInput);
    }
    
    private void OnAimPerformed(InputAction.CallbackContext context)
    {
        _cameraController.EnableAimCamera();
        _animationController.SetAimMode(true);
    }

    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        _cameraController.DisableAimCamera();
        _animationController.SetAimMode(false);
    }
    
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        var targetHit = _cameraController.GetRaycastHit(_camera);
        _weapon.Shoot(targetHit);
        
    }
    
    private void OnMoved()
    {
        _animationController.SetSpeedState(_movementController.WalkSpeed);
        _movementController.SetSpeed(_movementController.WalkSpeed);
    }

    private void OnStayed()
    {
        _animationController.SetSpeedState(0);
    }
    
    

    
    
}