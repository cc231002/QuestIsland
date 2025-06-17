using UnityEngine;

public class FloatingHeart : MonoBehaviour
{
    public float floatAmplitude = 0.5f;  // How far the heart floats up and down
    public float floatFrequency = 1f;    // How fast the floating motion is

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;  // Remember the starting position
    }

    void Update()
    {
        // Floating motion using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the heart (assuming player has tag "Player")
        if (other.CompareTag("Player"))
        {
            // Make the heart disappear
            gameObject.SetActive(false);
            // Alternatively, destroy it if no longer needed:
            // Destroy(gameObject);
            //Besi put here health logic increase here
        }
    }
}
