﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float _nextFire = 0;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _player;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerUpActive = false;
    private bool _isShieldPowerUpActive = false;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
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
    }

    public void Damage()
    {
        if (_isShieldPowerUpActive == true)
        {
            _isShieldPowerUpActive = false;
            _playerShield.SetActive(false);
            return;
        }
        else
        {
            _lives--;
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotTime());
    }

    public void SpeedPowerUpActive()
    {
        _isSpeedPowerUpActive = true;
        _speed *= _speedMultiplyer;
        StartCoroutine(SpeedPowerUp());
    }

    public void ShieldPowerUpActive()
    {
        _isShieldPowerUpActive = true;
        _playerShield.SetActive(true);
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
}
