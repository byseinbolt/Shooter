using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerCameraController))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerController : MonoBehaviour
{
    private PlayerAnimationController _animationController;
    private PlayerMovementController _movementController;
    private PlayerCameraController _cameraController;
    private PlayerWeaponController _weaponController;
    private PlayerInput _input;
    private Camera _camera;

    
    private void Awake()
    {
        _camera = Camera.main;
        _animationController = GetComponent<PlayerAnimationController>();
        _movementController = GetComponent<PlayerMovementController>();
        _cameraController = GetComponent<PlayerCameraController>();
        _weaponController = GetComponent<PlayerWeaponController>();
        _input = new PlayerInput();
    }

    private void Start()
    {
        _input.Player.Enable();
        _input.Player.Aim.performed += OnAimPerformed;
        _input.Player.Aim.canceled += OnAimCanceled;
        _input.Player.Shoot.performed += _weaponController.OnShoot;
        _movementController.Stay += OnStay;
        _movementController.Moved += OnMoved;
        _movementController.Run += OnRun;

    }

    
    private void OnDestroy()
    {
        _input.Player.Aim.performed -= OnAimPerformed;
        _input.Player.Aim.canceled -= OnAimCanceled;
        _input.Player.Shoot.performed -= _weaponController.OnShoot;
        _input.Player.Disable();
        
        _movementController.Stay -= OnStay;
        _movementController.Moved -= OnMoved;
        _movementController.Run -= OnRun;
    }



    private void Update()
    {
        _weaponController.Initialize(_cameraController.GetRaycastTarget(_camera));
        
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
    
  
    
    private void OnMoved()
    {
        _animationController.SetSpeedState(_movementController.WalkSpeed);
        _movementController.SetSpeed(_movementController.WalkSpeed);
    }

    private void OnStay()
    {
        _animationController.SetSpeedState(_movementController.StaySpeed);
    }
    
    private void OnRun()
    {
       _animationController.SetSpeedState(_movementController.RunSpeed);
       _movementController.SetSpeed(_movementController.RunSpeed);
    }


    
    
}