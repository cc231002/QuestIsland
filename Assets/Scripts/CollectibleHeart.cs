using UnityEngine;

public class CollectibleHeart : MonoBehaviour
{
    public string heartID; // must be unique per heart

    void Start()
    {
        // If this heart was already collected, disable it
        if (PlayerPrefs.GetInt("Collected_" + heartID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("Collected_" + heartID, 1);
    }
}
