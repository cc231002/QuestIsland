using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private const string ButtonClickEvent = "Play_ButtonClick";
    private const float SceneLoadDelay = 0.4f;

    public void StartGame()
    {
        PlayClickSound();
        StartCoroutine(LoadSceneWithDelay());
    }

    public void QuitGame()
    {
        PlayClickSound();
        StartCoroutine(QuitWithDelay());
    }

    private void PlayClickSound()
    {
        AkUnitySoundEngine.PostEvent(ButtonClickEvent, gameObject);
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(SceneLoadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(SceneLoadDelay);
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
