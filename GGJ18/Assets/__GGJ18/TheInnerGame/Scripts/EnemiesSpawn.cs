using UnityEngine;
using MEC;
using System.Collections.Generic;

public class EnemiesSpawn : MonoBehaviour {

    [SerializeField]
    EnemyPool[] _enemiesPool;

    [SerializeField]
    float _minSpawnPositionX;

    [SerializeField]
    float _maxSpawnPositionX;

    [SerializeField]
    float _spawnPositionZ;

    [SerializeField]
    float _minDelay;

    [SerializeField]
    float _maxDelay;

    List<Enemy> _enemyList = new List<Enemy>();

    CoroutineHandle? _spawnCoroutine = null;

    public void Initialize()
    {
        for (int i = 0; i < _enemiesPool.Length; ++i)
        {
            _enemiesPool[i].Initialize();
        }
    }

	public void StartSpawn()
    {
        Spawn();
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine.HasValue)
        {
            Timing.KillCoroutines(_spawnCoroutine.Value);
        }
    }

    private void Spawn()
    {
        float chances = 0;
        for (int i = 0; i < _enemiesPool.Length; ++i)
        {
            chances += _enemiesPool[i].Chances;
        }

        float random = Random.Range(0, chances);
        for (int i = 0; i < _enemiesPool.Length; ++i)
        {
            chances -= _enemiesPool[i].Chances;
            if (chances <= random)
            {
                Enemy enemy = _enemiesPool[i].Activate(new Vector3(
                        Random.Range(_minSpawnPositionX, _maxSpawnPositionX),
                        2f,
                        _spawnPositionZ));

                _enemyList.Add(enemy);
                break;
            }
        }

        _spawnCoroutine = Timing.CallDelayed(
                Random.Range(_minDelay, _maxDelay),
                Spawn);
    }

    public void Kill(Enemy enemy)
    {
        enemy.Kill();
        _enemyList.Remove(enemy);
    }

    public void Dispose()
    {
        for (int i = 0; i < _enemyList.Count; ++i)
        {
            Kill(_enemyList[i]);
        }
    }
}
