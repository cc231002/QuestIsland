using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private const string ButtonClickEvent = "Play_ButtonClick";
    private const float SceneLoadDelay = 0.3f; // Adjust depending on your sound length

    public void StartGame()
    {
        PlayClickSound();
        StartCoroutine(LoadSceneWithDelay());

    }

    public void RestartGame()
    {
        PlayClickSound();
        StartCoroutine(RestartWithDelay());
        HeartManager.Instance.ResetHearts();
        PlayerPrefs.DeleteAll();

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

    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSeconds(SceneLoadDelay);
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(SceneLoadDelay);
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
