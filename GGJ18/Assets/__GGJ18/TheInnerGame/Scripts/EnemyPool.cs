using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPool {

    [SerializeField]
    private Enemy _enemyPrefab;

    [SerializeField]
    private int _poolSize;

    [SerializeField]
    private float _chances;

    private Enemy[] _enemies;

    public float Chances {
        get {
            return _chances;
        }
    }

    public void Initialize()
    {
        _enemies = new Enemy[_poolSize];
        for (int i = 0; i < _poolSize; ++i)
        {
            _enemies[i] = GameObject.Instantiate(_enemyPrefab);
            _enemies[i].Initialize(this);
        } 
    }

    public Enemy Activate(Vector3 position)
    {
        Enemy enemy = Pick();
        if (enemy != null)
        {
            enemy.Activate(position);
        }

        return enemy;
    }

    public Enemy Pick()
    {
        for (int i = 0; i < _enemies.Length; ++i)
        {
            if (_enemies[i] != null)
            {
                Enemy enemy = _enemies[i];
                _enemies[i] = null;

                return enemy;
            }
        }

        return null;
    }

    public void ReturnToPool(Enemy enemy)
    {
        for (int i = 0; i < _enemies.Length; ++i)
        {
            if (_enemies[i] == null)
            {
                enemy.gameObject.SetActive(false);
                _enemies[i] = enemy;
                return;
            }
        }
    }
}
