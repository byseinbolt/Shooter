using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] 
    private Enemy _enemyPrefab;
    
    [SerializeField]
    private Transform _spawnPoint;
    
    [SerializeField]
    private float _radius;

    [SerializeField]
    private int _enemiesCount = 3;
    

    public void Initialize(Transform enemiesTarget)
    {
        for (int i = 0; i < _enemiesCount; i++)
        {
            var enemy = Instantiate(_enemyPrefab);
            var randomPosition = _spawnPoint.transform.position + Random.insideUnitSphere * _radius;
            randomPosition.y = 0;
            enemy.transform.position = randomPosition;
            enemy.Initialize(enemiesTarget);
        }
    }
    

}