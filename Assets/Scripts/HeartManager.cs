using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log($"Heart lost! Remaining: {CurrentHearts}");

            if (CurrentHearts <= 0)
            {
                HandleGameOver();
            }
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
    
    public void GainHeart()
{
    CurrentHearts = Mathf.Min(CurrentHearts + 1, maxHearts); 
    Debug.Log("Gained a heart!");
}

   
}
