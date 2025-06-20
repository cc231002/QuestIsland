// using UnityEngine;
// using System.Collections;
// using UnityEngine.SceneManagement;

// public class HeartManager : MonoBehaviour
// {
//     public static HeartManager Instance { get; private set; }

//     [SerializeField] private int maxHearts = 3;
//     public int CurrentHearts { get; private set; }

//     private void Awake()
//     {
//         // Ensure only one instance exists
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;
//         DontDestroyOnLoad(gameObject);
//         CurrentHearts = maxHearts;
//     }

//     public void LoseHeart()
//     {
//         if (CurrentHearts > 0)
//         {
//             CurrentHearts--;

//             // 🔊 Play heart lose sound
//             AkUnitySoundEngine.PostEvent("Play_HeartLose", gameObject);

//             Debug.Log($"Heart lost! Remaining: {CurrentHearts}");

//             if (CurrentHearts <= 0)
//             {
//                 HandleGameOver();
//             }
//         }
//     }

//     public void GainHeart()
//     {
//         if (CurrentHearts < maxHearts)
//         {
//             CurrentHearts++;

//             // 🔊 Play heart gain sound
//             AkUnitySoundEngine.PostEvent("Play_Heartup", gameObject);

//             Debug.Log("Gained a heart!");
//         }
//     }

//     public void ResetHearts()
//     {
//         CurrentHearts = maxHearts;
//     }

//     private void HandleGameOver()
//     {
//         Debug.Log("Game Over!");
//         SceneManager.LoadScene("GameOver");
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    public static HeartManager Instance { get; private set; }

    [SerializeField] private int maxHearts = 3;
    public int CurrentHearts { get; private set; }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentHearts = maxHearts;
    }

    public void LoseHeart()
    {
        if (CurrentHearts > 0)
        {
            CurrentHearts--;

            // 🔊 Play heart lose sound
            AkUnitySoundEngine.PostEvent("Play_HeartLose", gameObject);

            Debug.Log($"Heart lost! Remaining: {CurrentHearts}");

            if (CurrentHearts <= 0)
            {
                HandleGameOver();
            }
        }
    }

    public void GainHeart()
    {
        if (CurrentHearts < maxHearts)
        {
            CurrentHearts = Mathf.Min(CurrentHearts + 1, maxHearts);

            // 🔊 Play heart gain sound
            AkUnitySoundEngine.PostEvent("Play_Heartup", gameObject);

            Debug.Log("Gained a heart!");
        }
    }

    public void ResetHearts()
    {
        CurrentHearts = maxHearts;
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver");
    }
}
