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
    private GameObject _explosion;
    private AudioSource _explosionSoundEffect;
    private Animator _enemyDestroyed;
    private SpawnManager _spawnManager;
    private Player _player;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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
        }
    }

    private void BossAttackBehavior()
    {
        if (Time.time > _canFire)
        {
            _fireRate = 2.5f;
            _canFire = Time.time + _fireRate;
            StartCoroutine(BossShootingBehavior(_enemyLaser, 3, 5.0f));
            /*GameObject enemyLaser = Instantiate(_enemyLaser, transform.position + new Vector3(0, -1.15f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }*/
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
            _enemyDestroyed.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionSoundEffect.Play();
            transform.gameObject.tag = "EnemyDestroyed";
            Destroy(this.gameObject, 2.4f);
        }
    }
}
