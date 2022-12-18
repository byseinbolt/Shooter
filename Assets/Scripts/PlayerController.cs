using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerCameraController))]
[RequireComponent(typeof(UnitAnimationController))]
[RequireComponent(typeof(PlayerWeaponController))]
[RequireComponent(typeof(HealthHandler))]
public class PlayerController : MonoBehaviour
{
    private UnitAnimationController _animationController;
    private PlayerMovementController _movementController;
    private PlayerCameraController _cameraController;
    private PlayerWeaponController _weaponController;
    private HealthHandler _healthHandler;
    private PlayerInput _input;
    private Camera _camera;

    
    private void Awake()
    {
        _camera = Camera.main;
        _animationController = GetComponent<UnitAnimationController>();
        _movementController = GetComponent<PlayerMovementController>();
        _cameraController = GetComponent<PlayerCameraController>();
        _weaponController = GetComponent<PlayerWeaponController>();
        _healthHandler = GetComponent<HealthHandler>();
        _input = new PlayerInput();
    }

    private void Start()
    {
        _input.Player.Enable();
        _input.Player.Aim.performed += OnAimPerformed;
        _input.Player.Aim.canceled += OnAimCanceled;
        _input.Player.Shoot.performed += _weaponController.OnShoot;
        
        _healthHandler.Died += OnDied;
        _movementController.SpeedChanged += OnSpeedChanged;
        _movementController.Jump += OnJump;
        _movementController.FreeFall += OnFreeFall;
        _movementController.Grounded += OnGrounded;

    }
    
    private void OnDestroy()
    {
        _input.Player.Aim.performed -= OnAimPerformed;
        _input.Player.Aim.canceled -= OnAimCanceled;
        _input.Player.Shoot.performed -= _weaponController.OnShoot;
        _input.Player.Disable();
        
        _healthHandler.Died -= OnDied;
        _movementController.SpeedChanged -= OnSpeedChanged;
        _movementController.Jump -= OnJump;
        _movementController.FreeFall -= OnFreeFall;
        _movementController.Grounded -= OnGrounded;
    }
    
    private void Update()
    {
        _weaponController.Initialize(_cameraController.GetRaycastTarget(_camera));
        _movementController.JumpAndGravity(_input.Player.Jump.WasPressedThisFrame());
        _movementController.GroundCheck();
        
        var moveInput = _input.Player.Move.ReadValue<Vector2>();
        _movementController.Move(moveInput, _input.Player.Sprint.IsPressed(), _camera);
        
        if (_cameraController.IsAimMode)
        {
            _cameraController.RotateInAimMode(_camera);
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
        _cameraController.UpdateCameraRotation(lookInput);
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

    private void OnSpeedChanged()
    {
        _animationController.SetSpeedState(_movementController.AnimationBlend);
        _animationController.SetMotionSpeedState(_movementController.Magnitude);
    }
    
    private void OnGrounded(bool isGrounded)
    {
        _animationController.SetGroundedState(isGrounded);
    }

    private void OnFreeFall(bool isFreeFall)
    {
        _animationController.SetFreeFallState(isFreeFall);
    }

    private void OnJump(bool isJumping)
    {
       _animationController.SetJumpState(isJumping);
    }
    
    private void OnDied()
    {
        _animationController.Died();
        enabled = false;
        _input.Player.Disable();
    }
}