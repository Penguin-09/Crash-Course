using System.Security;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public ParticleSystem tireSmokeParticleLeft;
    public ParticleSystem tireSmokeParticleRight;
    private AudioSource playerAudio;
    public AudioClip collisionSound1;
    public AudioClip collisionSound2;
    public AudioClip collisionSound3;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI GameOverText;

    public float speed = 20000.0f;
    public float turnSpeed = 75.0f;

    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;
    private int timeCounter = 0;
    private float countdownTimer = 5.0f;
    private int enemyCollisionCount = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        InvokeRepeating("IncrementTimer", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // Only allow movement, turning and tire smoke when grounded
        if (isGrounded)
        {
            // Move the player forward or backward based on vertical input
            playerRb.AddForce(transform.forward * Time.deltaTime * speed * verticalInput);

            // Turn the player depending on current forward/backward speed. (Rob helped a lot with this part)
            float forwardSpeed = Vector3.Dot(playerRb.linearVelocity, transform.forward);
            float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / 10f);
            float direction = forwardSpeed >= 0f ? 1f : -1f;
            transform.Rotate(Vector3.up, direction * turnSpeed * horizontalInput * speedFactor * Time.deltaTime);

            // Play tire smoke particles if the player is moving
            if (playerRb.linearVelocity.magnitude > 0.05f)
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

        // If the player is colliding with an enemy, reduce the countdown timer.
        if (enemyCollisionCount > 0)
        {
            countdownTimer -= Time.deltaTime;
        } else {
            countdownTimer = 5.0f;
            CountdownText.text = "";
        }

        // Only display the timer when the countdown is active
        if (countdownTimer < 5.0f) {
            CountdownText.text = countdownTimer.ToString("F2") + "s";
        }

        // Check for game over conditions
        if (countdownTimer <= 0f || transform.position.y < -1f)
        {
            CountdownText.text = "";
            GameOver();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Play a random collision sound
        switch (Random.Range(0, 3))
        {
            case 0:
                playerAudio.PlayOneShot(collisionSound1, 1f);
                break;
            case 1:
                playerAudio.PlayOneShot(collisionSound2, 1f);
                break;
            case 2:
                playerAudio.PlayOneShot(collisionSound3, 1f);
                break;
        }

        // Set isGrounded to true when the player touches the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Increment enemy collision count when entering enemy hitbox
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CatchHitbox"))
        {
            enemyCollisionCount++;
        }
    }

    // Decrement enemy collision count when exiting enemy hitbox
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CatchHitbox"))
        {
            enemyCollisionCount--;
        }
    }
    
    // Set isGrounded to false when the player leaves the ground
    void OnCollisionExit(Collision collision)
    {
        // Set isGrounded to false when the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Increment the time counter and update the UI text
    void IncrementTimer()
    {
        timeCounter++;
        TimeText.text = "Time: " + timeCounter.ToString() + "s";
    }

    // Stop game simulation when timer reaches 0 or when player falls below map
    void GameOver()
    {
        GameOverText.text = "Game Over!";
        Time.timeScale = 0f;
    }
}
