using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    [SerializeField]
    private float _topOfScreen = 7.0f;
    [SerializeField]
    private float _fireRate = 3.0f;
    [SerializeField]
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator EnemyDestroyed;
    private AudioSource _explosionSoundEffect;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionSoundEffect = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }
        float randomX = Random.Range(-9.5f, 9.5f);

        if (_explosionSoundEffect == null)
        {
            Debug.LogError("Audio Source is null");
        }

        transform.position = new Vector3(randomX, _topOfScreen, 0);

        EnemyDestroyed = GetComponent<Animator>();

        if (EnemyDestroyed == null)
        {
            Debug.LogError("EnemyDestroyed is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5.4)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, _topOfScreen, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            EnemyDestroyed.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _explosionSoundEffect.Play();
            Destroy(this.gameObject, 2.4f);
        }


        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.Score(Random.Range(5, 10));
            }
            EnemyDestroyed.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _explosionSoundEffect.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.4f);
            
            
        }
    }
}
