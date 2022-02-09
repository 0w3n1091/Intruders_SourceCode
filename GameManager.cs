using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject target;
    public GameObject weaponPrefab;
    public GameObject enemyPrefab;
    public HighScore highscore;
    public List<Transform> enemySpawnPoints = new List<Transform>();
    public List<Transform> weaponSpawnPoints = new List<Transform>();
    public static GameManager Instance;
    public int waveIncrement = 0;
    private Vector2 lastWeaponPosition;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnWeapon();
        StartCoroutine(SpawnEnemyWave(20));
    }

    public IEnumerator SpawnEnemyWave(int aDelay)
    {
        while (true)
        {
            AudioManager.Instance.newWave.Play();

            for (int i = 0; i < 2 + waveIncrement; i++)
            {
                Vector2 spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].transform.position;
                GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                enemy.GetComponent<EnemyAI>().target = target.transform;
                enemy.GetComponent<Enemy>().score = highscore;
            }

            waveIncrement += 2;

            yield return new WaitForSeconds(aDelay);
        }
    }

    public void SpawnWeapon()
    {
        Vector2 spawnPosition = weaponSpawnPoints[Random.Range(0, weaponSpawnPoints.Count)].transform.position;

        if (spawnPosition == lastWeaponPosition)
        {
            SpawnWeapon();
        }
        else
        {
            GameObject weapon = (GameObject)Instantiate(weaponPrefab, spawnPosition, Quaternion.identity);
            lastWeaponPosition = spawnPosition;
        }
    }
}
