using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerGameController : TeamTheDream.Singleton<InnerGameController> {

    [SerializeField]
    private int _startLifes = 3;

    [SerializeField]
    InnerGameView _view;

    [SerializeField]
    EnemiesSpawn _enemiesSpawn;

    public event System.Action<int> OnPlayerKillsEnemy;
    public event System.Action OnPlayerTakeDamage;
    public event System.Action OnPlayerDie;

    private bool _isActive;
    private int _lifes;

    public bool IsActive {
        get {
            return _isActive;
        }

        set {
            _isActive = value;
            Cursor.visible = !_isActive;
        }
    }

    // Use this for initialization
    private void Start () {
        IsActive = true;
        _enemiesSpawn.Initialize();
        InitializeGame();
    }

    public void TakeDamage(Enemy enemy)
    {
        if (_lifes > 0)
        {
            --_lifes;

            _view.Damage();

            if (_lifes > 0)
            {
                //Effects
                if (OnPlayerTakeDamage != null) OnPlayerTakeDamage();
            }
            else
            {
                _enemiesSpawn.StopSpawn();
                if (OnPlayerDie != null) OnPlayerDie();
                EndGame();
            }
        }

        _enemiesSpawn.Kill(enemy);
    }

    public void KillEnemy(Enemy enemy)
    {
        _enemiesSpawn.Kill(enemy);
        if (OnPlayerKillsEnemy != null) OnPlayerKillsEnemy(enemy.Points);
    }

    private void InitializeGame()
    {
        _lifes = _startLifes;
        _view.Hide(StartGame);
    }

    private void StartGame()
    {
        _enemiesSpawn.StartSpawn();
    }

    private void EndGame()
    {
        _view.Show(() =>
        {
            _enemiesSpawn.Dispose();
            InitializeGame();
        });
    }

    public void SwitchInnerGameActive()
    {
        IsActive = !_isActive;
    }

}
