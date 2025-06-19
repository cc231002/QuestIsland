using TMPro;
using UnityEngine;

public class OceanDangerZone : MonoBehaviour
{
    public TextMeshPro warningText3D;
    private bool playerInside = false;
    private float damageTimer = 0f;
    public float damageInterval = 3f;

    private void Start()
    {
        if (warningText3D != null)
            warningText3D.gameObject.SetActive(false); // make sure it's hidden on start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (warningText3D != null)
            {
                warningText3D.gameObject.SetActive(true);
                warningText3D.text = "WARNING: Dangerous Waters!";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (warningText3D != null)
            {
                warningText3D.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                HeartManager.Instance.LoseHeart();
                damageTimer = 0f;
            }
        }
    }
}
