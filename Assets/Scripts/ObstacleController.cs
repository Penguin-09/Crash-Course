using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject Crate;
    public GameObject Barrel;
    public GameObject cone;

    // Generate a random spawn position within the defined range
    Vector3 GenerateSpawnPosition()
    {
        float xPosition = Random.Range(-58, 58);
        float zPosition = Random.Range(-58, 58);
        return new Vector3(xPosition, 3, zPosition);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Spawn 15 random obstacles at the start of the game
        for (int i = 0; i < 15; i++)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    Instantiate(Crate, GenerateSpawnPosition(), Crate.transform.rotation);
                    break;
                case 1:
                    Instantiate(Barrel, GenerateSpawnPosition(), Barrel.transform.rotation);
                    break;
                case 2:
                    Instantiate(cone, GenerateSpawnPosition(), cone.transform.rotation);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the obstacle if it falls below the map
        if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }
}
