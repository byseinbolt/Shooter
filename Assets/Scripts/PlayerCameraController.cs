using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCameraController : MonoBehaviour
{
    public bool IsAimMode => _isAimMode; 
    
    [SerializeField]
    private GameObject _cinemachineCameraTarget;

    [SerializeField]
    private Transform _aimTarget;
   
   [SerializeField]
   private float _topClamp = 70.0f;

   [SerializeField]
   private float _bottomClamp = -30.0f;

   [SerializeField]
   private CinemachineVirtualCamera _playerAimCamera;

   [SerializeField]
   private float _mouseSensitivity;

   [SerializeField]
   private LayerMask _layerMask;

   [SerializeField]
   private Rig _aimRig;
   
   private float _cinemachineTargetYaw;
   private float _cinemachineTargetPitch;
   
   private bool _isAimMode;
   
   

   public void CameraRotation(Vector2 lookInput)
   {
       _cinemachineTargetYaw += lookInput.x *  Time.fixedDeltaTime * _mouseSensitivity;
       _cinemachineTargetPitch -= lookInput.y  * Time.fixedDeltaTime * _mouseSensitivity;
      
       _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
       _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
      
       _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
           _cinemachineTargetYaw, 0.0f);
   }
   public void PlayerAimRotation(Camera mainCamera)
   {
       var mouseWorldPosition = GetRaycastHit(mainCamera);
       
       var aimTarget = mouseWorldPosition.point;
       aimTarget.y = transform.position.y;
       var aimDirection = (aimTarget - transform.position).normalized;
        
       transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
   }
   
   public void EnableAimCamera()
   {
       _playerAimCamera.gameObject.SetActive(true);
       _isAimMode = true;
       _aimRig.weight = 1;
   }

   public void DisableAimCamera()
   {
       _playerAimCamera.gameObject.SetActive(false);
       _isAimMode = false;
       _aimRig.weight = 0;
   }
   
   public RaycastHit GetRaycastHit(Camera mainCamera)
   {
       var centerScreenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
       var ray = mainCamera.ScreenPointToRay(centerScreenPoint);
       if (Physics.Raycast(ray, out var hit, 500f, _layerMask))
       {
           _aimTarget.position = hit.point;
           return hit;
       }
       return new RaycastHit();
       
   }
   private float ClampAngle(float lfAngle, float lfMin, float lfMax)
   {
       if (lfAngle < -360f) lfAngle += 360f;
       if (lfAngle > 360f) lfAngle -= 360f;
       return Mathf.Clamp(lfAngle, lfMin, lfMax);
   }
}
