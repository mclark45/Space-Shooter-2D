using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;

    private AudioSource _explosionSoundEffect;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionSoundEffect = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        if (_explosionSoundEffect == null)
        {
            Debug.LogError("Audio Source is Null");
        }
        transform.position = new Vector3(0, 3.5f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 15f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            GameObject explode = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(explode.gameObject, 2.4f);
            Destroy(collision.gameObject);
           // _spawnManager.StartSpawning();
            _spawnManager.BossSpawnBehavior();
            Destroy(this.gameObject, 0.25f);

            _explosionSoundEffect.Play();
        }
    }
}
