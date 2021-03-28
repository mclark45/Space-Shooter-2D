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
    private GameObject _enemyShield;


    private int randomMove;
    private int _shieldSpawn;
    private bool _isEnemyShieldActive = false;


    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator EnemyDestroyed;
    private AudioSource _explosionSoundEffect;

    private bool _isActive = false;  // Enemy death animation


    void Start()
    {

        _isActive = false;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _explosionSoundEffect = GetComponent<AudioSource>();
        EnemyDestroyed = GetComponent<Animator>();
        randomMove = Random.Range(0, 3);
        _shieldSpawn = Random.Range(0, 2);

        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }

        if (_explosionSoundEffect == null)
        {
            Debug.LogError("Audio Source is null");
        }

        if (EnemyDestroyed == null)
        {
            Debug.LogError("EnemyDestroyed is null");
        }

        float randomX = Random.Range(-9.5f, 9.5f);
        transform.position = new Vector3(randomX, _topOfScreen, 0);
    }

    void Update()
    {
        CalculateMovement(_shieldSpawn);

        if (Time.time > _canFire)
       {
           if (_isActive == false)
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
    }



    void CalculateMovement(int shield)
    {
        EnemyMovement();

        if (shield == 1)
        {
            _enemyShield.SetActive(true);
            _isEnemyShieldActive = true;
        }
        else
        {
            _enemyShield.SetActive(false);
            _isEnemyShieldActive = false;
        }

        if (transform.position.y < -5.4)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, _topOfScreen, 0);
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                if (_isEnemyShieldActive == false)
                { 
                    player.Damage();
                }
            }

            if (_isEnemyShieldActive == true)
            {
                _shieldSpawn = 0;
                _isEnemyShieldActive = false;
                _enemyShield.SetActive(false);
                _explosionSoundEffect.Play();
            }
            else
            {
                EnemyDestroyed.SetTrigger("OnEnemyDeath");
                _isActive = true;
                _enemySpeed = 0;
                _explosionSoundEffect.Play();
                Destroy(this.gameObject, 2.4f);
            }
        }


        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                if (_isEnemyShieldActive == false)
                {
                    _player.Score(Random.Range(5, 10));
                }

                if (_isEnemyShieldActive == true)
                {
                    _shieldSpawn = 0;
                    _isEnemyShieldActive = false;
                    _enemyShield.SetActive(false);
                    _explosionSoundEffect.Play();
                }
                else
                {
                    EnemyDestroyed.SetTrigger("OnEnemyDeath");
                    _isActive = true;
                    _enemySpeed = 0;
                    _explosionSoundEffect.Play();
                    Destroy(GetComponent<Collider2D>());
                    transform.gameObject.tag = "EnemyDestroyed";
                    Destroy(this.gameObject, 2.4f);
                }
            }
        }

        if (other.CompareTag("EnemyLaser"))
        {
            return;
        }
    }

    public void EnemyMovement()
    {
        switch (randomMove)
        {
            case 0:
                transform.Translate(new Vector3(0, -1, 0) * _enemySpeed * Time.deltaTime);
                    break;
            case 1:
                transform.Translate(new Vector3(1, -1, 0) * _enemySpeed * Time.deltaTime);
                    break;
            case 2:
                transform.Translate(new Vector3(-1, -1, 0) * _enemySpeed * Time.deltaTime);
                    break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }
}
