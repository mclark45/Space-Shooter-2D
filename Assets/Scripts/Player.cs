using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplyer = 15.0f;
    [SerializeField]
    private float _upperBoundary = 0;
    [SerializeField]
    private float _lowerBoundary = -3.8f;
    [SerializeField]
    private float _leftBoundary = -11.3f;
    [SerializeField]
    private float _rightBoundary = 11.3f;
    [SerializeField]
    private float _fireRate = 1.0f;
    [SerializeField]
    private float _nextFire = -1;
    [SerializeField]
    private float _thrusterReady = 10f;
    [SerializeField]
    private float _nextThruster = -1;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shieldLives = 3;
    [SerializeField]
    private int _ammoRemaining = 15;


    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _homingMissle;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    private UIManager _uiManager;

    private ThrusterBar _thrusterBar;

    private SpawnManager _spawnManager;

    private SpriteRenderer _shieldVisuals;



    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerUpActive = false;
    private bool _isShieldPowerUpActive = false;
    private bool _isHomingMisslePowerUpActive = false;
    private bool _isDebuffPowerUpActive = false;

    [SerializeField]
    private AudioClip _laserShot;
    [SerializeField]
    private AudioClip _explosionSoundEffect;
    [SerializeField]
    private AudioClip _powerupSoundEffect;

    private AudioSource _playerSoundEffects;

    public CameraControl cameraControl;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _thrusterBar = GameObject.Find("Canvas").GetComponent<ThrusterBar>();
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

        if (_thrusterBar == null)
        {
            Debug.LogError("Thruster Bar is Null");
        }
    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _nextThruster)
        {
            if (_isSpeedPowerUpActive == false && _isDebuffPowerUpActive == false)
            {
                Thrusters();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }

    public void Thrusters()
    {
        _nextThruster = Time.time + _thrusterReady;
        _thrusterBar.UseThruster(1);
        StartCoroutine(Timer());
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
    }

    void FireLaser()
    {
         _nextFire = Time.time + _fireRate;
        if (_ammoRemaining > 0)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShot, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _ammoRemaining --;
                _uiManager.UpdateAmmoCount(_ammoRemaining);
            }
            else if (_isHomingMisslePowerUpActive == true)
            {
                Instantiate(_homingMissle, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                _ammoRemaining--;
                _uiManager.UpdateAmmoCount(_ammoRemaining);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                _ammoRemaining --;
                _uiManager.UpdateAmmoCount(_ammoRemaining);
            }

            _playerSoundEffects.clip = _laserShot;

            _playerSoundEffects.Play();
        }
    }

    public void Damage()
    {
        _playerSoundEffects.clip = _explosionSoundEffect;
        cameraControl.StartCameraShake();
        

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
        _speed = _speedMultiplyer;
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

    public void HomingMissleActive()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();
        _isHomingMisslePowerUpActive = true;
        StartCoroutine(HomingMissle());
    }

    public void DebuffActive()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();
        _isDebuffPowerUpActive = true;
        _speed = -5.0f;
        StartCoroutine(Debuff());
    }

    public void CollectedAmmoPowerUp()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();

        if (_ammoRemaining <= 12)
        {
            _ammoRemaining += 3;
            _uiManager.UpdateAmmoCount(_ammoRemaining);
        }
        else
        {
            _ammoRemaining = 15;
            _uiManager.UpdateAmmoCount(_ammoRemaining);
        }
    }

    public void CollectedHealthPowerup()
    {
        _playerSoundEffects.clip = _powerupSoundEffect;
        _playerSoundEffects.Play();

        if (_lives == 3)
        {
            return;
        }
        else if (_lives == 2)
        {
            _rightEngine.SetActive(false);
            _lives++;
            _uiManager.UpdateLives(_lives);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(false);
            _lives++;
            _uiManager.UpdateLives(_lives);
        }
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
        _speed = 5f;
    }

    IEnumerator HomingMissle()
    {
        yield return new WaitForSeconds(5.0f);
        _isHomingMisslePowerUpActive = false;
    }

    IEnumerator Debuff()
    {
        yield return new WaitForSeconds(5.0f);
        _isDebuffPowerUpActive = false;
        _speed = 5.0f;

    }

    IEnumerator Timer()
    {
        _speed += 5;
        yield return new WaitForSeconds(5.0f);
        _speed -= 5;
        yield return null;
    }

    public void Score(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
