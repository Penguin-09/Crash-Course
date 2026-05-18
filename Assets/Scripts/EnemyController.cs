using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    public ParticleSystem tireSmokeParticleLeft;
    public ParticleSystem tireSmokeParticleRight;

    public float speed;
    public float turnSpeed = 5.0f;

    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");

        // Set a random speed for the enemy
        speed = Random.Range(15000.0f, 25000.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            // Rotate the enemy towards the player (pulled this from StackOverflow)
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Move the enemy forward at a constant speed
            enemyRb.AddForce(transform.forward * Time.deltaTime * speed);

            // Play tire smoke particles if the player is moving
            if (enemyRb.linearVelocity.magnitude > 0.05f)
            {
                if (!tireSmokeParticleLeft.isPlaying) tireSmokeParticleLeft.Play();
                if (!tireSmokeParticleRight.isPlaying) tireSmokeParticleRight.Play();
            } else {
                if (tireSmokeParticleLeft.isPlaying) tireSmokeParticleLeft.Stop();
                if (tireSmokeParticleRight.isPlaying) tireSmokeParticleRight.Stop();
            }
        } else if (tireSmokeParticleLeft.isPlaying || tireSmokeParticleRight.isPlaying) {
            // Stop tire smoke particles if the player is not grounded
            tireSmokeParticleLeft.Stop();
            tireSmokeParticleRight.Stop();
        }

        // Destroy the enemy if it falls below the map
        if (transform.position.y < -3)
        {
            Destroy(gameObject);
        }
    }

    // Set isGrounded to true when the enemy touches the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    // Set isGrounded to false when the enemy leaves the ground
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
