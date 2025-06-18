using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    [Header("Rotation speed in degrees per second")]
    public Vector3 rotationSpeed = new Vector3(0f, 10f, 0f);

    void Update()
    {
        // Rotate the object based on rotationSpeed and deltaTime
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
