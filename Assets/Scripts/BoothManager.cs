using UnityEngine;

public class BoothManager : MonoBehaviour
{
    public GameObject[] booths;  // Assign all 6 booth GameObjects here in inspector

    void Start()
    {
        int currentBooth = PlayerPrefs.GetInt("CurrentBooth", 1);

        if (currentBooth < 1 || currentBooth > booths.Length)
        {
            Debug.LogWarning("Current booth index out of range, resetting to 1");
            currentBooth = 1;
            PlayerPrefs.SetInt("CurrentBooth", currentBooth);
        }

        for (int i = 0; i < booths.Length; i++)
        {
            booths[i].SetActive(i == (currentBooth - 1));  // Activate only current booth
        }
    }
}
