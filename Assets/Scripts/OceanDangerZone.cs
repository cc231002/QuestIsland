using TMPro;
using UnityEngine;

public class OceanDangerZone : MonoBehaviour
{
    public TextMeshPro warningText3D; // 3D warning text to show when player enters
    private bool playerInside = false; // tracks if the player is in the danger zone
    private float damageTimer = 0f; // timer for applying damage
    public float damageInterval = 3f; // how often player takes damage in seconds

    private void Start()
    {
        // make sure the warning text is hidden at the beginning
        if (warningText3D != null)
            warningText3D.gameObject.SetActive(false); // make sure it's hidden on start
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if the thing entering is the player
        if (other.CompareTag("Player"))
        {
            playerInside = true; // now we know player is inside
            if (warningText3D != null)
            {
                // show the warning and set the message
                warningText3D.gameObject.SetActive(true);
                warningText3D.text = "WARNING: Dangerous Waters!";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // when the player leaves the zone
        if (other.CompareTag("Player"))
        {
            playerInside = false; // player is no longer inside
            if (warningText3D != null)
            {
                warningText3D.gameObject.SetActive(false); // hide the warning text
            }
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            // count up the time player has spent in the danger zone
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                // damage the player (remove a heart)
                HeartManager.Instance.LoseHeart();
                damageTimer = 0f; // reset timer after applying damage
            }
        }
    }
}
