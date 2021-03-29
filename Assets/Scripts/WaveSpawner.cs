using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { Spawning, Waiting, Counting}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int _nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float _waveCounter;
    private float _searchCounter = 1f;
    [SerializeField]
    private Transform _enemyContainer;

    [SerializeField]
    private Text _waveNumber;
    [SerializeField]
    private Text _bossLives;


    private SpawnState _state = SpawnState.Counting;

    private void Start()
    {
        _waveCounter = timeBetweenWaves;
        _waveNumber.gameObject.SetActive(false);
        _bossLives.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_state == SpawnState.Waiting)
        {
            if (EnemyIsAlive() == false)
            {
                BeginRound();
                return;
            }
            else
            {
                return;
            }
        }

        if (_waveCounter <= 0 && _state != SpawnState.Spawning)
        {
            StartCoroutine(SpawnWave(waves[_nextWave]));
        }
        else
        {
            _waveCounter -= Time.deltaTime;
        }

        if (_state == SpawnState.Counting && _nextWave < 5)
        {
            _waveNumber.gameObject.SetActive(true);
            _waveNumber.text = "Wave " + (_nextWave + 1);
        }
        else
        {
            _waveNumber.gameObject.SetActive(false);
        }
    }

    private void BeginRound()
    {
        _state = SpawnState.Counting;
        _waveCounter = timeBetweenWaves;

        _nextWave++;
    }

    private bool EnemyIsAlive()
    {
        _searchCounter -= Time.deltaTime;
        if (_searchCounter <= 0f)
        {

            _searchCounter = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        _state = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        _state = SpawnState.Waiting;


        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        if (_enemy.name == "Enemy")
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            Transform newEnemy = Instantiate(_enemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
        else if (_enemy.name == "Boss")
        {
            Vector3 posToSpawn = new Vector3(0, 7.0f, 0);
            Instantiate(_enemy, posToSpawn, Quaternion.identity);
            _bossLives.gameObject.SetActive(true);
        }
    }

}
