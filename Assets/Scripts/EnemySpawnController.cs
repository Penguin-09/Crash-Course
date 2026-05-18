using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject enemyVehicle;
    
    private float startDelay = 4.0f;
    private float interval = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnEnemy", startDelay, interval);
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

        // Only decrease spawn interval if it is greater than 0.5
        if (interval > 0.5f)
        {
            interval -= 0.5f;
        }
    }
}
