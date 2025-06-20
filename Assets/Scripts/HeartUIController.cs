using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartUIController : MonoBehaviour
{
    public static HeartUIController Instance { get; private set; }
    public List<GameObject> hearts;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdateHearts();
    }

    private void Update()
    {
        UpdateHearts();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReparentToNewCanvas();
    }

    private void ReparentToNewCanvas()
{
    GameObject canvasObj = GameObject.FindGameObjectWithTag("GameCanvas"); // or Find by name

    if (canvasObj == null)
    {
        //Debug.LogWarning("No canvas found to reparent HeartUI.");
        return;
    }

    Canvas targetCanvas = canvasObj.GetComponent<Canvas>();
    if (targetCanvas == null)
    {
        //Debug.LogWarning("Target object doesn't have a Canvas component.");
        return;
    }

    // Set HeartUI as child of the new canvas
    transform.SetParent(canvasObj.transform, false);

    // Update Canvas-specific properties
    Canvas heartCanvas = GetComponentInParent<Canvas>();
    if (heartCanvas != null && targetCanvas.renderMode == RenderMode.ScreenSpaceCamera)
    {
        heartCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        heartCanvas.worldCamera = targetCanvas.worldCamera;
        heartCanvas.planeDistance = targetCanvas.planeDistance;
    }
    else if (heartCanvas != null && targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
    {
        heartCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        heartCanvas.worldCamera = null;
    }

    //Debug.Log($"HeartUI successfully reparented to canvas: {canvasObj.name}");
}


    private void UpdateHearts()
    {
        if (HeartManager.Instance == null) return;

        int currentHearts = HeartManager.Instance.CurrentHearts;

        for (int i = 0; i < hearts.Count; i++)
        {
            bool isActive = i < currentHearts;
            hearts[i].SetActive(isActive);
        }
    }
}
