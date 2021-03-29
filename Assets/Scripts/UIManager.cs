using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoCount;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _bossLives;

    private GameManager _gameManager;

    private SpawnManager _spawnManager;


    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _bossLives.gameObject.SetActive(false);
        
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is Null");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
    }

    public void UpdateScore(int playerscore)
    {
        _scoreText.text = "Score: " + playerscore;
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCount.text = "Ammo: " + ammoCount + "/15";
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            _gameManager.GameOver();
            StartCoroutine(flicker());
        }



        IEnumerator flicker()
        {
            while (2 > 1)
            {
                _gameOverText.gameObject.SetActive(true);
                _restartText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);

                _gameOverText.gameObject.SetActive(false);
               // _restartText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public void BossTextActive()
    {
        _bossLives.gameObject.SetActive(true);
    }

    public void UpdateBossLives(int Lives)
    {
        _bossLives.text = "Boss Lives: " + Lives;

        if (Lives == 0)
        {
            _gameManager.GameOver();
            _spawnManager.OnPlayerDeath();
            StartCoroutine(flicker());
        }

        IEnumerator flicker()
        {
            while (2 > 1)
            {
                _gameOverText.text = "You Won!";
                _gameOverText.gameObject.SetActive(true);
                _restartText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);

                _gameOverText.gameObject.SetActive(false);
                // _restartText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
