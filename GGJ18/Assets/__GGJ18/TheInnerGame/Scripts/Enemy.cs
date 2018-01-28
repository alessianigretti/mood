using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int _maxLifes = 1;

    [SerializeField]
    private int _points = 1;

    [SerializeField]
    private Ease _pathCurve = Ease.Linear;

    [SerializeField]
    private float _arcSize = 0;

    [SerializeField]
    private int _halfWaypointsNumber = 0;

    [SerializeField]
    private float _speed;

    [SerializeField]
    TintWhite _tinterWhiter;

    [SerializeField]
    Transform _spriteEnemy;

    [SerializeField]
    Transform _diedAnimation0;

    [SerializeField]
    Transform _diedAnimation1;

    private int _lifes;

    private Tweener _tweenerX;
    private Tweener _tweenerZ;

    private EnemyPool _pool;

    public int Points {
        get {
            return _points;
        }
    }

    public void Initialize(EnemyPool pool)
    {
        _pool = pool;
        gameObject.SetActive(false);
    }

    public void Activate (Vector3 position) {
        _lifes = _maxLifes;

        Transform transform = GetComponent<Transform>();
        Transform cameraTrasform = Camera.main.transform;

        _spriteEnemy.gameObject.SetActive(true);
        _diedAnimation0.gameObject.SetActive(false);
        _diedAnimation1.gameObject.SetActive(false);

        transform.position = position;

        if (_halfWaypointsNumber > 0)
        {
            int currentWaypointIndex = 0;
            Vector3[] halfWaypoints = new Vector3[_halfWaypointsNumber];

            bool leftSide = false;
            if (transform.position.x > 0)
            {
                leftSide = true;
            }
            else if (transform.position.x == 0)
            {
                leftSide = Random.Range(0, 2) == 0;
            }

            float segmentSizeX = transform.position.x / _halfWaypointsNumber;
            float segmentSizeZ = (transform.position.z - cameraTrasform.position.z) / _halfWaypointsNumber;
            for (int i = 0; i < _halfWaypointsNumber; ++i)
            {
                halfWaypoints[i] = new Vector3(
                    transform.position.x - segmentSizeX * (i + 0.5f) + (leftSide ? -_arcSize : _arcSize),
                    0,
                    transform.position.z - segmentSizeZ * (i + 0.5f));

                leftSide = !leftSide;
            }

            GoToWaypoint(halfWaypoints, currentWaypointIndex);
        }
        else
        {
            GoToPlayer();
        }
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        if (_tweenerX != null)
        {
            _tweenerX = null;
            _tweenerX.Kill();
        }
        if (_tweenerZ != null)
        {
            _tweenerZ = null;
            _tweenerZ.Kill();
        }

        _pool.ReturnToPool(this);
    }

    private void GoToWaypoint(Vector3[] waypoints, int currentWaypointIndex)
    {
        float distance = Vector3.Distance(transform.position, waypoints[currentWaypointIndex]);
        float duration = distance / _speed;

        _tweenerX = transform.DOMoveX(waypoints[currentWaypointIndex].x, duration).SetEase(_pathCurve);
        _tweenerZ = transform.DOMoveZ(waypoints[currentWaypointIndex].z, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            ++currentWaypointIndex;
            if (currentWaypointIndex < _halfWaypointsNumber) {
                GoToWaypoint(waypoints, currentWaypointIndex);
            }
            else
            {
                GoToPlayer();
            }
        });
    }

    private void GoToPlayer()
    {
        Vector3 destinyPosition = Camera.main.transform.position;
        destinyPosition.z += 1;
        float distance = Vector3.Distance(transform.position, destinyPosition);
        float duration = distance / _speed;

        _tweenerX = transform.DOMoveX(destinyPosition.x, duration).SetEase(_pathCurve);
        _tweenerZ = transform.DOMoveZ(destinyPosition.z, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            InnerGameController.Instance.TakeDamage(this);
        });
    }

    public void TakeDamage()
    {
        --_lifes;

        if (_lifes > 0)
        {
            //Effects
            _tinterWhiter.whiteSprite();
            Timing.CallDelayed(0.1f,() => 
            {
                _tinterWhiter.normalSprite();
            });

        }
        else
        {
            if (_tweenerX != null)
            {
                _tweenerX = null;
                _tweenerX.Kill();
            }
            if (_tweenerZ != null)
            {
                _tweenerZ = null;
                _tweenerZ.Kill();
            }

            //Effects
            _tinterWhiter.whiteSprite();
            Timing.CallDelayed(0.1f, () =>
            {
                _tinterWhiter.normalSprite();
            });

            _spriteEnemy.gameObject.SetActive(false);
            _diedAnimation0.gameObject.SetActive(true);
            _diedAnimation1.gameObject.SetActive(true);

            Timing.CallDelayed(0.3f,()=>{
                InnerGameController.Instance.KillEnemy(this);
            });

           
        }
    }
}