using UnityEngine;

public class FootstepSurfaceSwitcher : MonoBehaviour
{
    private string currentSurface = "";

    private void OnTriggerEnter(Collider other)
    {
        ZoneSurfaceSwitch zone = other.GetComponent<ZoneSurfaceSwitch>();
        if (zone != null)
        {
            string newSurface = zone.surfaceType;

            if (newSurface != currentSurface)
            {
                currentSurface = newSurface;

                // Set the Wwise switch: "Walk" is the group, e.g., "Grass" is the value
                AkUnitySoundEngine.SetSwitch("Walk", newSurface, gameObject);

                // Restart footsteps (optional)
                AkUnitySoundEngine.PostEvent("Stop_Walk", gameObject);
                AkUnitySoundEngine.PostEvent("Play_Walk", gameObject);
            }
        }
    }
}
