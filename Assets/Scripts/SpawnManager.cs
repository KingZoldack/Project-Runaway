using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Properties")]
    [Tooltip("Set how many enemies you would like to spawn. There are only 3 types of enemies.")]
    [SerializeField]
    GameObject[] _enemies;
    [Tooltip("Set how quickly you would like enemies to spawn.")]
    [SerializeField]
    float _spawnRate;
    [Tooltip("set the rotation for the bottom enemy.")]
    [SerializeField]
    Vector3 _flipEnemy = new Vector3(0, 0, 180);

    [Header("Offset Properties")]
    [Tooltip("Offset for where the bottom enemies should spawn.")]
    [SerializeField]
    float _bottomSpawnOffset = -1.2f;
    [Tooltip("Offset for enemy for when they spawn at the bottom.")]
    [SerializeField]
    float _singleEnemyOffset = 0 , _doubleEnemyOffset = 1f, _tripleEnemyOffset = 2f;
    
    int _randomEnemyNumber;
    int _randomSpawn;

    Vector3 _spawnPosition;

    private void Update()
    {
        if (GameManager.Instance.CanSpawn())
            StartCoroutine(StartSpawnRoutine());
    }

    public IEnumerator StartSpawnRoutine()
    {
        GameManager.Instance.CantSpawn();

        while (!GameManager.Instance.isGameOver())
        {
            SpawnEnemies();
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    void SpawnEnemies()
    {
        _spawnPosition = transform.position;
        _randomEnemyNumber = Random.Range(0, _enemies.Length); //Spawns single (0), double (1) or triple enemy (2+)
        _randomSpawn = Random.Range(0, 2); //0 = Spawn at top, 1 = Spawn at bottom

        if (_randomSpawn == 0)
        {
            handleFlippedSprite(false);

            Instantiate(_enemies[_randomEnemyNumber], _spawnPosition, Quaternion.identity);
        }
        else
        {
            _spawnPosition.y = _bottomSpawnOffset;

            switch (_randomEnemyNumber) //Offset flipped enemies
            {
                case 0:
                    _spawnPosition.x -= _singleEnemyOffset;
                    break;
                case 1:
                    _spawnPosition.x -= _doubleEnemyOffset;
                    break;
                case 2:
                    _spawnPosition.x -= _tripleEnemyOffset;
                    break;
                default:
                    _spawnPosition.x -= _tripleEnemyOffset;
                    break;
            }

            handleFlippedSprite(true);

            GameObject bottomEnemy = Instantiate(_enemies[_randomEnemyNumber], _spawnPosition, transform.rotation);
            bottomEnemy.transform.eulerAngles = _flipEnemy;
        }
    }

    void handleFlippedSprite (bool isFlipped)
    {
        foreach (var enemy in _enemies)
        {
            var enemySpriteRenderer = enemy.GetComponentsInChildren<SpriteRenderer>();

            foreach (var spriteRenderer in enemySpriteRenderer)
            {
                spriteRenderer.flipX = isFlipped;
            }
        }
    }
}
