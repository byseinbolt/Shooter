using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCameraController : MonoBehaviour
{
    public bool IsAimMode { get; private set; }

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

   public void UpdateCameraRotation(Vector2 lookInput)
   {
       _cinemachineTargetYaw += lookInput.x  * _mouseSensitivity;
       _cinemachineTargetPitch -= lookInput.y  * _mouseSensitivity;
      
       _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
       _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
      
       _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
           _cinemachineTargetYaw, 0.0f);
   }
   public void RotateInAimMode(Camera mainCamera)
   {
       var mouseWorldPosition = GetRaycastTarget(mainCamera);
       
       var aimTarget = mouseWorldPosition.point;
       aimTarget.y = transform.position.y;
       var aimDirection = (aimTarget - transform.position).normalized;
        
       transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
   }
   
   public void EnableAimCamera()
   {
       _playerAimCamera.gameObject.SetActive(true);
       IsAimMode = true;
       _aimRig.weight = 1;
   }

   public void DisableAimCamera()
   {
       _playerAimCamera.gameObject.SetActive(false);
       IsAimMode = false;
       _aimRig.weight = 0;
   }
   
   public RaycastHit GetRaycastTarget(Camera mainCamera)
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
