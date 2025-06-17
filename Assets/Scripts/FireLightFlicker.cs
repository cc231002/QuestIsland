using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    public Light fireLight;         
    public float minIntensity = 1.5f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 0.1f; // How fast the flicker changes

    private float targetIntensity;

    void Start()
    {
        if (fireLight == null)
            fireLight = GetComponent<Light>();

        targetIntensity = fireLight.intensity;
    }

    void Update()
    {
        // Smoothly move the intensity toward a new random target
        targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, Random.Range(0f, 1f));
        fireLight.intensity = Mathf.Lerp(fireLight.intensity, targetIntensity, flickerSpeed);
    }
}
