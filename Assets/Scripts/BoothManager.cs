using UnityEngine;
using TMPro;

public class BoothManager : MonoBehaviour
{
    public GameObject[] booths;  // Assign all 6 booth GameObjects here in inspector
    public TMP_Text boothProgressText;
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
        UpdateBoothProgressText(currentBooth);

    }

// This method should be called when a booth is completed
    void UpdateBoothProgressText(int currentBooth)
    {
        int completed = Mathf.Clamp(currentBooth - 1, 0, booths.Length - 1);
        boothProgressText.text = $"Completed Trivia Quests: {completed}/6";
    }
}
