using UnityEngine;
using System.Collections;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject enemyVehicle;
    
    private float startDelay = 4.0f;
    private float interval = 5.0f;
    private float minimumInterval = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    // Spawn enemies at an decreasing interval
    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(startDelay);

        // Continuously spawn enemies
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(interval);
            
            // Decrease the interval if it's above the minimum interval
            interval = Mathf.Max(minimumInterval, interval - 0.5f);
        }
    }

    // Generate a random spawn position within the defined range
    Vector3 GenerateSpawnPosition()
    {
        float xPosition = Random.Range(-55, 55);
        float zPosition = Random.Range(-55, 55);
        return new Vector3(xPosition, 3, zPosition);
    }

    // Spawn an enemy vehicle at a random position
    void SpawnEnemy()
    {
        Instantiate(enemyVehicle, GenerateSpawnPosition(), enemyVehicle.transform.rotation);
    }
}
