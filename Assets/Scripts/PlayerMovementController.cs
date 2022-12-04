using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    public event Action Moved;
    public event Action Stay;
    public event Action Run;
   

    public float StaySpeed => 0f;
    [field: SerializeField] public float WalkSpeed { get;private set;}
    [field: SerializeField] public float RunSpeed { get; private set;}
    
    
    [SerializeField]
    private float _rotationSmoothTime = 0.12f;
    
    private CharacterController _characterController;

    private float _speed;
    private float _rotationVelocity;
    private bool _rotateOnMove;
    
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    public void Move(Vector2 moveInput, Camera mainCamera)
    {
        var inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        
        if (moveInput != Vector2.zero && Input.GetKey(KeyCode.LeftShift))
        {
            MoveCharacter(inputDirection, mainCamera);
            Run?.Invoke();
        }
        else if (moveInput != Vector2.zero)
        {
           MoveCharacter(inputDirection,mainCamera);
           Moved?.Invoke();
        }
        else
        {
            _speed = 0f;
            Stay?.Invoke();
        }
    }

    public void SetRotateOnMove(bool isMoving)
    {
        _rotateOnMove = isMoving;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
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
        _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime));
    }

   
}