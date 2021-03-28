using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _bossEnemy;



    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnHomingMissleRoutine());
        StartCoroutine(SpawnHealthPowerUpRoutine());
        StartCoroutine(SpawnShieldPowerUpRoutine());
        StartCoroutine(SpawnAmmoPowerUpRoutine());
    }

    public void BossSpawnBehavior()
    {
        if (_stopSpawning == false)
        {
            StartCoroutine(BossSpawn());
        }
    }

    IEnumerator BossSpawn()
    {
        yield return new WaitForSeconds(3.0f);

        Vector3 posToSpawn = new Vector3(0, 7.0f, 0);
        Instantiate(_bossEnemy, posToSpawn, Quaternion.identity);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }  
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10f, 20f));
        }
    }

    IEnumerator SpawnAmmoPowerUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            Instantiate(_powerUps[6], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10f, 20f));
        }
    }

    IEnumerator SpawnHealthPowerUpRoutine()
    {
        yield return new WaitForSeconds(30f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(_powerUps[4], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(30f, 60f));
        }
    }

    IEnumerator SpawnShieldPowerUpRoutine()
    {
        yield return new WaitForSeconds(15f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            Instantiate(_powerUps[3], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(30f, 60f));
        }
    }

    IEnumerator SpawnHomingMissleRoutine()
    {
        yield return new WaitForSeconds(20f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.0f, 0);
            Instantiate(_powerUps[5], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(30f, 45f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
