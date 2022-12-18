using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    public event Action SpeedChanged;
    public event Action<bool> Jump;
    public event Action<bool> FreeFall;
    public event Action<bool> Grounded; 

    public float AnimationBlend { get; private set; }
    public float Magnitude { get; private set; }

    [Header("Movement")]
    [SerializeField]
    private float _walkSpeed;
    
    [SerializeField]
    private float _runSpeed;
    
    [SerializeField]
    private float _rotationSmoothTime = 0.12f;
    
    [SerializeField]
    private float _speedChangeRate = 10.0f;
    
    [Header("Jump And Gravity")]
    [SerializeField]
    private float _jumpHeight = 1.2f;
    
    [SerializeField]
    private float _gravity = -15.0f;
    
    [SerializeField]
    private float _jumpTimeout = 0.50f;
    
    [SerializeField]
    private float _fallTimeout = 0.15f;
    
    [Header("Player Grounded")]
    [SerializeField] 
    private bool _grounded = true;
    
    [SerializeField]
    private float _groundedOffset = -0.14f;
    
    [SerializeField]
    private float _groundedRadius = 0.28f;
    
    [SerializeField]
    private LayerMask _groundLayers;
    
    private CharacterController _characterController;
    private float _speed;
    private bool _rotateOnMove;

    private float _rotationVelocity;
    private float _verticalVelocity;
    private  float _terminalVelocity = 53.0f;
    
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;
    }
    
    public void Move(Vector2 moveInput, bool isRunning, Camera mainCamera)
    {
         var targetSpeed = isRunning ? _runSpeed : _walkSpeed;
         if (moveInput == Vector2.zero)
         {
             targetSpeed = 0f;
         }
         AccelerateOrDecelerateSpeed(targetSpeed);

         AnimationBlend = Mathf.Lerp(AnimationBlend, targetSpeed, Time.deltaTime * _speedChangeRate);
         if (AnimationBlend < 0.01f) AnimationBlend = 0f;
         
         var inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
         SpeedChanged?.Invoke();
         
         MoveCharacter(inputDirection,mainCamera);
         
    }
    
    public void SetRotateOnMove(bool isMoving)
    {
        _rotateOnMove = isMoving;
    }
    
    public void JumpAndGravity(bool shouldJump)
    {
        if (_grounded)
        {
            _fallTimeoutDelta = _fallTimeout;
            Jump?.Invoke(false);
            FreeFall?.Invoke(false);
            
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }
            
            if (shouldJump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                Jump?.Invoke(true);
            }
            
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = _jumpTimeout;
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                FreeFall?.Invoke(true);
            }
        }
        
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }
    
    public void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
            transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
            QueryTriggerInteraction.Ignore);
        
        Grounded?.Invoke(_grounded);
    }
    
    private void AccelerateOrDecelerateSpeed(float targetSpeed)
    {
        var currentHorizontalSpeed =
            new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

        var speedOffset = 0.1f;
        Magnitude = 1f;
        
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * Magnitude,
                Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
    }
    
    private void MoveCharacter(Vector3 inputDirection, Camera mainCamera)
    {
        var targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                             + mainCamera.transform.eulerAngles.y;
        var rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
            ref _rotationVelocity, _rotationSmoothTime);

        if (_rotateOnMove)
        {
            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
        
        var targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                  new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

   
}