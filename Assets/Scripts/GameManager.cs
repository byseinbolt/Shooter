using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _enemyManager;

    [SerializeField]
    private PlayerController _player;
    
    [SerializeField]
    private bool _shouldLockCursor;
    
    private void Awake()
    {
        _enemyManager.Initialize(_player.transform);
        if (_shouldLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
       
    }
}