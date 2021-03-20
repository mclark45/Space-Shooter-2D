using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;


    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is Null");
        }
    }

    public void UpdateScore(int playerscore)
    {
        _scoreText.text = "Score: " + playerscore;
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
                _restartText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
