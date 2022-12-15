using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    public event Action SpeedChanged;

    public float Speed { get; private set; }

    [SerializeField]
    private float _walkSpeed;
    
    [SerializeField]
    private float _runSpeed;
    
    [SerializeField]
    private float _rotationSmoothTime = 0.12f;
    
    private CharacterController _characterController;

    private float _rotationVelocity;
    private bool _rotateOnMove;
    
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    public void Move(Vector2 moveInput, bool isRunning, Camera mainCamera)
    {
        var inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        var shouldMove = moveInput != Vector2.zero;
        Speed = shouldMove && isRunning ? _runSpeed : shouldMove ? _walkSpeed : 0f;

        SpeedChanged?.Invoke();
        MoveCharacter(inputDirection,mainCamera);
    }

    public void SetRotateOnMove(bool isMoving)
    {
        _rotateOnMove = isMoving;
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
        _characterController.Move(targetDirection.normalized * (Speed * Time.deltaTime));
    }

   
}