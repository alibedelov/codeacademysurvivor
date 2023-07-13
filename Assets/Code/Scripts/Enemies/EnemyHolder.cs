using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Managers;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField] private List<List<GameObject>> _objects;
    [SerializeField] private float _groupSpawnRate = 2f;
    [SerializeField] private int _enemiesPerSpawn;
    [SerializeField] private int _activeObjectsThreshold = 20;
    [SerializeField] private int _objectsToSpawn = 30;
    [SerializeField] public List<GameObject> _activeObjects;

    private int _currentAngleStep;
    private int _angleStep = 20;
    private Camera _mainCamera;
    private float _randomX;
    private float _randomY;

    [SerializeField] private Transform _player;
    [SerializeField] private List<EnemyBase> _enemies;
    [SerializeField] private EnemyBase _dogs;
    [SerializeField] private bool _wave;
    [SerializeField] private int _enemyIndex;
    [SerializeField] private bool _bossComming;


    public bool BossAlive
    {
        get { return _bossComming; }
        set { _bossComming = value; }
    }

    private void Awake()
    {
        _objects = new List<List<GameObject>>();
        _objects.Add(new List<GameObject>());
        _objects.Add(new List<GameObject>());
        _objects.Add(new List<GameObject>());
    }

    private void OnEnable()
    {
        TimeManager.Instance.OnFirstMinuteEvent += PlantZombieSpawn;
        TimeManager.Instance.OnTenSecondEvent += ChangeSpawnRate;
        TimeManager.Instance.OnTwoMinuteEvent += FirstWaveStart;
        TimeManager.Instance.OnFourMinuteEvent += SecondWave;
        TimeManager.Instance.OnFiveMinuteEvent += FirstBossComing;
    }

    private void PlantZombieSpawn()
    {
        
        StartCoroutine(SpawnPlants());
    }

    private IEnumerator SpawnPlants()
    {
        float circleRadius = 12f;
        if (_objects.Count <= 2 || _objects[2].Count < 8)
        {
            Debug.Log("Not enough objects to spawn plants");
            yield break;
        }
        
        // Positions
        Vector2 basePosition = new Vector2(_player.position.x+10, _player.position.y+10); // This can be changed to the desired base position
        Vector2 offset1 = new Vector2(1, 0); // Horizontal offset
        Vector2 offset2 = new Vector2(0, 1); // Vertical offset

        // Spawn the plants
        for (int i = 0; i < 4; i++)
        {
            GameObject go = _objects[2][0];
            _objects[2].RemoveAt(0);
            go.transform.position = basePosition + offset1 * (i % 2) + offset2 * (i / 2);
            go.SetActive(true);
        }

        yield return new WaitForSeconds(2f); // Wait for 2 seconds between spawns
        basePosition = new Vector2(_player.position.x-10,_player.position.y-10);
        // Spawn the second batch of plants
        for (int i = 0; i < 4; i++)
        {
            GameObject go = _objects[2][0];
            _objects[2].RemoveAt(0);
           
            go.transform.position = basePosition + offset1 * (i % 2) + offset2 * (i / 2) + offset1 * 3; // Offset for second batch
            go.SetActive(true);
        }
        
        yield return new WaitForSeconds(2f); // Wait for 2 seconds between spawns
        basePosition = new Vector2(_player.position.x-10,_player.position.y);
        // Spawn the second batch of plants
        for (int i = 0; i < 4; i++)
        {
            GameObject go = _objects[2][0];
            _objects[2].RemoveAt(0);
           
            go.transform.position = basePosition + offset1 * (i % 2) + offset2 * (i / 2) + offset1 * 3; // Offset for second batch
            go.SetActive(true);
        }
    }

    private void FirstBossComing()
    {
        _bossComming = true;
        for (int i = _activeObjects.Count - 1; i >= 0; i--)
        {
            EnemyMonoBase enemy = _activeObjects[i].GetComponent<EnemyMonoBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(5000); //Assuming this damage is enough to kill the enemy
            }
        }
        
    }

    private void SecondWave()
    {
        _enemyIndex = 1;
        StartCoroutine(StartWave());
    }

    private void FirstWaveStart()
    {
        StartCoroutine(StartWave());
        StartCoroutine(SpawnEnemiesCoroutine());
    }


    private IEnumerator StartWave()
    {
        _wave = true;
        _groupSpawnRate = 0.2f;
        yield return new WaitForSeconds(30f);
        _groupSpawnRate = 1.8f;
        _enemyIndex = 0;
        _wave = false;
    }

    /// <summary>
    /// İf killed size < 20% of Active Objects in scene, so Up Spawn Rate
    /// else if killed size >50% of Active Objects in scene => down spawn rate
    /// P.S Best Solution for now (18.05.23)
    /// </summary>
    /// <param name="killedSize">Count of enemies that was killed in last 10 seconds of game</param>
    private void ChangeSpawnRate(int killedSize)
    {
        if (_wave) return;
        else
        {
            if (killedSize < 15 && killedSize > 10)
                _groupSpawnRate = 2.2f;
            else if (killedSize > 15 && killedSize < 20)
                _groupSpawnRate = 1.5f;
            else if (killedSize > 20 && killedSize < 25)
                _groupSpawnRate = 1.1f;
            else if (killedSize > 25 && killedSize < 40)
                _groupSpawnRate = 0.7f;
        }
    }

    public void DestroyingEnemy(GameObject obj, int index)
    {
        if (_activeObjects.Contains(obj))
        {
            _objects[index].Add(obj);
            _activeObjects.Remove(obj);
            StartCoroutine(ObjDisable(obj));
        }
    }

    IEnumerator ObjDisable(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        obj.GetComponent<EnemyMonoBase>().OnEnemyDie -= DestroyingEnemy;
        obj?.SetActive(false);
    }

    private void Start()
    {
        _mainCamera = Camera.main;

        for (int i = 0; i < 1600; i++)
        {
            GameObject go = _enemies[0].Activate();

            go.transform.SetParent(transform);
            _objects[0].Add(go);
            go.GetComponent<EnemyMonoBase>().OnEnemyDie += DestroyingEnemy;
            go.SetActive(false);
        }

        for (int i = 0; i < 800; i++)
        {
            GameObject go = _enemies[1].Activate();

            go.transform.SetParent(transform);
            _objects[1].Add(go);
            go.GetComponent<EnemyMonoBase>().OnEnemyDie += DestroyingEnemy;
            go.SetActive(false);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject go = _enemies[2].Activate();
            go.transform.SetParent(transform);
            _objects[2].Add(go);
            go.GetComponent<EnemyMonoBase>().OnEnemyDie += DestroyingEnemy;
            go.SetActive(false);
        }

        StartCoroutine(SpawnEnemiesCoroutine());
    }


    public IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            if (_activeObjects.Count < _activeObjectsThreshold && _objects[_enemyIndex].Count >= _objectsToSpawn)
            {
                float circleRadius = 7f;
                float groupRadius = 3f;

                int groups = _objectsToSpawn / _enemiesPerSpawn;

                for (int i = 0; i < groups; i++)
                {
                    while (_bossComming)
                    {
                        yield return null; // Wait until the next frame before checking again
                    }
                    for (int j = 0; j < _enemiesPerSpawn; j++)
                    {
                        float angle = (_angleStep * i + (360f / _enemiesPerSpawn) * j) % 360;
                        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                            Mathf.Sin(angle * Mathf.Deg2Rad));
                        Vector2 spawnPosition = (Vector2)_player.transform.position + direction * circleRadius;
                        Vector2 offset = new Vector2(Random.Range(-groupRadius, groupRadius),
                            Random.Range(-groupRadius, groupRadius));
                        spawnPosition += offset;

                        if (_objects.Count > 0)
                        {
                            _activeObjects.Add(_objects[_enemyIndex][_objects.Count - 1]);
                            _objects[_enemyIndex][_objects.Count - 1].SetActive(true);
                            _objects[_enemyIndex][_objects.Count - 1].transform.position = spawnPosition;
                            _objects[_enemyIndex].Remove(_objects[_enemyIndex][_objects.Count - 1]);
                        }
                    }

                    

                    _currentAngleStep += _angleStep;
                    yield return new WaitForSeconds(_groupSpawnRate);
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}
