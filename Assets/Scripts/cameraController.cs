using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    
    private Vector3 offset = new Vector3(40, 60, 40);

    // LateUpdate is called once per frame after all Update functions have been called
    void LateUpdate()
    {
        // Set the camera's position to be the player's position + the offset
        transform.position = player.transform.position + offset;
    }
}
