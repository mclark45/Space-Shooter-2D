using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private float _fireRate = 3.0f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 5;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _explosion;
    private AudioSource _explosionSoundEffect;
    private Animator _enemyDestroyed;
    private SpawnManager _spawnManager;
    private Player _player;
    private bool _isActive = false;
    private bool _bossIsDefeated = false;


    void Start()
    {
        _bossIsDefeated = false;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyDestroyed = GetComponent<Animator>();
        _explosionSoundEffect = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        if (_explosionSoundEffect == null)
        {
            Debug.LogError("Audio Source is null");
        }

        if (_enemyDestroyed == null)
        {
            Debug.LogError("Enemy Animator is Null");
        }

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI manager is Null");
        }



        transform.position = new Vector3(0, 7f, 0);
    }

    private void Update()
    {
        CalculateMovement();
        BossAttackBehavior();

        Vector3 followPlayer = _player.transform.position - transform.position;
        float angle = (Mathf.Atan2(followPlayer.y, followPlayer.x) * Mathf.Rad2Deg) - -90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _speed);
    }

    private void CalculateMovement()
    {
        if (transform.position.y >= 4.0f)
        {
            transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(0, 0, 0) * _speed * Time.deltaTime);
            StartCoroutine(MoveSideToSide());
        }


        IEnumerator MoveSideToSide()
        {
            while (transform.position.y <= 4.0f)
            {
                yield return new WaitForSeconds(3.0f);
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, 4.0f, transform.position.z);
                yield return new WaitForSeconds(3.0f);
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, 4.0f, transform.position.z);
            }
        }
    }

    private void BossAttackBehavior()
    {
        float randomRate = Random.Range(1.0f, 2.5f);

        if (Time.time > _canFire)
        {
            if (_isActive == false)
            {
                _fireRate = randomRate;
                _canFire = Time.time + _fireRate;
                StartCoroutine(BossShootingBehavior(_enemyLaser, 3, _fireRate));
            }
        }
    }

    IEnumerator BossShootingBehavior(GameObject laser, int burstSize, float rateOfFire)
    {
        float laserDelay = 60f / rateOfFire;

        for (int i = 0; i < burstSize; i++)
        {
            GameObject enemyLaser = Instantiate(laser, transform.position , transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int a = 0; a < lasers.Length; a++)
            {
                lasers[a].AssignEnemy();
            }

            yield return new WaitForSeconds(laserDelay);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            _player.Score(Random.Range(15, 25));
            if (_lives >= 1)
            {
                _lives--;
                GameObject explode = Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(explode, 2.4f);
                _explosionSoundEffect.Play();
                _uiManager.UpdateBossLives(_lives);
                Destroy(other.gameObject);
            }

            if (_lives == 0)
            {
                _enemyDestroyed.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _explosionSoundEffect.Play();
                transform.gameObject.tag = "EnemyDestroyed";
                _isActive = true;
                Destroy(other.gameObject);
                Destroy(this.gameObject,2.4f);
            }
        }
    }
}
