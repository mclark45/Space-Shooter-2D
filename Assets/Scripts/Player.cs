using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplyer = 2.0f;
    [SerializeField]
    private float _upperBoundary = 0;
    [SerializeField]
    private float _lowerBoundary = -3.8f;
    [SerializeField]
    private float _leftBoundary = -11.3f;
    [SerializeField]
    private float _rightBoundary = 11.3f;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _nextFire = -1;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shieldLives = 3;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    private UIManager _uiManager;

    private SpawnManager _spawnManager;

    private SpriteRenderer _shieldVisuals;

    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerUpActive = false;
    private bool _isShieldPowerUpActive = false;

    [SerializeField]
    private AudioClip _laserShot;
    [SerializeField]
    private AudioClip _explosionSoundEffect;
    [SerializeField]
    private AudioClip _powerupSoundEffect;

    private AudioSource _playerSoundEffects;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _playerSoundEffects = GetComponent<AudioSource>();
        _shieldVisuals = _playerShield.GetComponent<SpriteRenderer>();

        if (_shieldVisuals == null)
        {
            Debug.LogError("Sprite Renderer is Null");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }

        if (_playerSoundEffects == null)
        {
            Debug.LogError("Audio Source is Null");
        }
    }

    
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
 

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _lowerBoundary, _upperBoundary), 0);

        if (transform.position.x >= _rightBoundary)
        {
            transform.position = new Vector3(_leftBoundary, transform.position.y, 0);
        }
        else if (transform.position.x <= _leftBoundary)
        {
            transform.position = new Vector3(_rightBoundary, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = 10f;
        }
        else if (_isSpeedPowerUpActive == false)
        {
            _speed = 5f;
        }
    }

    void FireLaser()
    {
         _nextFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShot, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _playerSoundEffects.clip = _laserShot;

        _playerSoundEffects.Play();
    }

    public void Damage()
    {
        _playerSoundEffects.clip = _explosionSoundEffect;

        if (_isShieldPowerUpActive == true)
        {
            if (_shieldLives == 3)
            {
                _shieldLives -= 1;
                _shieldVisuals.color = Color.green;
                return;
            }
            else if (_shieldLives == 2)
            {
                _shieldLives -= 1;
                _shieldVisuals.color = Color.red;
                return;
            }
            else if (_shieldLives == 1)
            {
                _isShieldPowerUpActive = false;
                _playerShield.SetActive(false);
                return;
            }
        }

            _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);

            _playerSoundEffects.Play();
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);

            _playerSoundEffects.Play();
        }

        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            _speed = 0;
            Destroy(this.gameObject, 1f);

            _playerSoundEffects.Play();
        }
    }

    public void TripleShotActive()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();
        _isTripleShotActive = true;
        StartCoroutine(TripleShotTime());
    }

    public void SpeedPowerUpActive()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();
        _isSpeedPowerUpActive = true;
        _speed *= _speedMultiplyer;
        StartCoroutine(SpeedPowerUp());
    }

    public void ShieldPowerUpActive()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();
        _isShieldPowerUpActive = true;
        _playerShield.SetActive(true);
        _shieldVisuals.color = Color.white;
        _shieldLives = 3;
    }

    IEnumerator TripleShotTime()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerUp()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedPowerUpActive = false;
        _speed /= _speedMultiplyer;
    }

    public void Score(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
