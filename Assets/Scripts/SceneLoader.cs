using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the player enters the trigger.")]
    public string sceneToLoad;

    [Tooltip("The trivia category this booth represents (e.g., Geography, Math).")]
    public string triviaCategory; // NEW look in the inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered booth. Loading scene: " + sceneToLoad + " with category: " + triviaCategory);
            PlayerPrefs.SetString("SelectedCategory", triviaCategory); // safe category
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
