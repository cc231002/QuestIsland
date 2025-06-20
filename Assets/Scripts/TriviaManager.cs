// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class TriviaManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class TriviaData { public TriviaCategories trivia_questions; }

//     [System.Serializable]
//     public class TriviaCategories
//     {
//         public CategoryData Geography, Math, Science, History, Sports, Psychology;
//     }

//     [System.Serializable]
//     public class CategoryData
//     {
//         public List<Question> Beginner, Intermediate, Hard;
//     }

//     [System.Serializable]
//     public class Question
//     {
//         public int number;
//         public string question;
//         public List<Option> options;
//         public string answer;
//     }

//     [System.Serializable]
//     public class Option
//     {
//         public string key;
//         public string value;
//     }

//     public TMP_Text categoryTextUI;
//     public TMP_Text questionTextUI;
//     public TMP_Text[] answerTextUIs;
//     public Button[] answerButtons;
//     public TMP_Text timerTextUI;
//     public Image timerFillBar;
//     public TMP_Text questionCountTextUI;

//     public float maxTimePerQuestion = 20f;
//     private float currentTime;
//     private bool isTimerRunning = false;
//     private float lastSecondPlayed = -1f; // For timer sound tracking

//     public Color normalColor = Color.green;

//     private string selectedCategory;
//     private List<Question> selectedQuestions = new List<Question>();
//     private int currentQuestionIndex = 0;

//     private int pressedWrongIndex = -1;

//     void Start()
//     {
//         selectedCategory = PlayerPrefs.GetString("SelectedCategory", "Geography");
//         categoryTextUI.text = selectedCategory + ":";

//         LoadQuestions();
//         ShowQuestion();
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Alpha1)) CheckAnswer("A", 0);
//         if (Input.GetKeyDown(KeyCode.Alpha2)) CheckAnswer("B", 1);
//         if (Input.GetKeyDown(KeyCode.Alpha3)) CheckAnswer("C", 2);
//         if (Input.GetKeyDown(KeyCode.Alpha4)) CheckAnswer("D", 3);

//         if (isTimerRunning)
//         {
//             currentTime -= Time.deltaTime;
//             if (currentTime < 0f) currentTime = 0f;

//             float ratio = currentTime / maxTimePerQuestion;
//             timerFillBar.fillAmount = ratio;
//             timerTextUI.text = Mathf.Ceil(currentTime).ToString();

//             int currentSecond = Mathf.CeilToInt(currentTime);
//             if (currentSecond != Mathf.CeilToInt(lastSecondPlayed))
//             {
//                 AkUnitySoundEngine.PostEvent("Play_Timer", gameObject);

//                 // Update RTPC in Wwise
//                  float invertedRTPC = maxTimePerQuestion - currentTime;
//                 AkUnitySoundEngine.SetRTPCValue("TimeLeft", invertedRTPC, gameObject);

//                 lastSecondPlayed = currentTime;
// }

//             if (currentTime <= 0f)
//             {
//                 isTimerRunning = false;
//                 Debug.Log("Time's up!");
//                 HeartManager.Instance.LoseHeart();

//                 HighlightAnswers(-1);
//                 StartCoroutine(NextQuestionAfterDelay(1f));
//             }
//         }
//     }

//     void LoadQuestions()
//     {
//         TextAsset jsonFile = Resources.Load<TextAsset>("trivia_question_bank");
//         if (jsonFile == null)
//         {
//             Debug.LogError("JSON file not found!");
//             return;
//         }

//         TriviaData triviaData = JsonUtility.FromJson<TriviaData>(jsonFile.text);
//         if (triviaData == null || triviaData.trivia_questions == null)
//         {
//             Debug.LogError("Failed to parse trivia data.");
//             return;
//         }

//         CategoryData cat = selectedCategory switch
//         {
//             "Geography" => triviaData.trivia_questions.Geography,
//             "Math" => triviaData.trivia_questions.Math,
//             "Science" => triviaData.trivia_questions.Science,
//             "History" => triviaData.trivia_questions.History,
//             "Sports" => triviaData.trivia_questions.Sports,
//             "Psychology" => triviaData.trivia_questions.Psychology,
//             _ => null
//         };

//         if (cat == null)
//         {
//             Debug.LogError("Invalid category: " + selectedCategory);
//             return;
//         }

//         if (cat.Beginner.Count > 0)
//             selectedQuestions.Add(cat.Beginner[Random.Range(0, cat.Beginner.Count)]);
//         if (cat.Intermediate.Count > 0)
//             selectedQuestions.Add(cat.Intermediate[Random.Range(0, cat.Intermediate.Count)]);
//         if (cat.Hard.Count > 0)
//             selectedQuestions.Add(cat.Hard[Random.Range(0, cat.Hard.Count)]);
//     }

//     void ShowQuestion()
//     {
//         currentTime = maxTimePerQuestion;
//         lastSecondPlayed = -1f;
//         isTimerRunning = true;
//         timerFillBar.fillAmount = 1f;
//         timerFillBar.color = normalColor;
//         timerTextUI.text = Mathf.Ceil(maxTimePerQuestion).ToString();

//         ResetAnswerButtonColors();
//         pressedWrongIndex = -1;

//         questionCountTextUI.text = currentQuestionIndex + 1 + "/" + selectedQuestions.Count;

//         if (currentQuestionIndex >= selectedQuestions.Count)
//         {
//             questionTextUI.text = "You finished all questions!";
//             questionCountTextUI.text = "";

//             foreach (var txt in answerTextUIs) txt.text = "";
//             foreach (var btn in answerButtons) btn.interactable = false;

//             StartCoroutine(LoadSceneAfterDelay(3f));
//             return;
//         }

//         Question q = selectedQuestions[currentQuestionIndex];
//         questionTextUI.text = q.question;

//         for (int i = 0; i < answerButtons.Length; i++)
//         {
//             if (i < q.options.Count)
//             {
//                 string optionKey = q.options[i].key;

//                 TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
//                 buttonText.text = $"{optionKey}: {q.options[i].value}";

//                 answerButtons[i].onClick.RemoveAllListeners();

//                 string capturedKey = optionKey;
//                 int capturedIndex = i;
//                 answerButtons[i].onClick.AddListener(() => CheckAnswer(capturedKey, capturedIndex));

//                 answerButtons[i].gameObject.SetActive(true);
//                 answerButtons[i].interactable = true;
//             }
//             else
//             {
//                 answerButtons[i].gameObject.SetActive(false);
//             }
//         }
//     }

//     void CheckAnswer(string selectedOption, int buttonIndex)
//     {
//         if (!isTimerRunning) return;

//         isTimerRunning = false;

//         Question currentQuestion = selectedQuestions[currentQuestionIndex];

//         if (selectedOption == currentQuestion.answer)
//         {
//             //Debug.Log("Correct!");
//             AkUnitySoundEngine.PostEvent("Play_Correct", gameObject);
//         }
//         else
//         {
//             //Debug.Log("Wrong!");
//             AkUnitySoundEngine.PostEvent("Play_Wrong", gameObject);
//             HeartManager.Instance.LoseHeart();
//             pressedWrongIndex = buttonIndex;
//         }

//         HighlightAnswers(pressedWrongIndex);
//         StartCoroutine(NextQuestionAfterDelay(0.8f));
//     }

//     void HighlightAnswers(int wrongPressedIndex)
//     {
//         Question currentQuestion = selectedQuestions[currentQuestionIndex];

//         for (int i = 0; i < answerButtons.Length; i++)
//         {
//             if (!answerButtons[i].gameObject.activeSelf) continue;

//             TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
//             string btnTextStr = btnText.text;

//             if (btnTextStr.StartsWith(currentQuestion.answer + ":"))
//             {
//                 answerButtons[i].GetComponent<Image>().color = Color.green;
//                 answerButtons[i].interactable = false;
//             }
//             else if (i == wrongPressedIndex)
//             {
//                 answerButtons[i].GetComponent<Image>().color = Color.red;
//                 answerButtons[i].interactable = false;
//             }
//             else
//             {
//                 answerButtons[i].interactable = false;
//             }
//         }
//     }

//     void ResetAnswerButtonColors()
//     {
//         foreach (var btn in answerButtons)
//         {
//             btn.GetComponent<Image>().color = Color.white;
//             btn.interactable = true;
//         }
//     }

//     IEnumerator NextQuestionAfterDelay(float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         currentQuestionIndex++;
//         ShowQuestion();
//     }

//     IEnumerator LoadSceneAfterDelay(float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         SceneManager.LoadScene("GameScene");
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TriviaManager : MonoBehaviour
{
    [System.Serializable] public class TriviaData { public TriviaCategories trivia_questions; }
    [System.Serializable] public class TriviaCategories
    {
        public CategoryData Geography, Math, Science, History, Sports, Psychology;
    }
    [System.Serializable] public class CategoryData
    {
        public List<Question> Beginner, Intermediate, Hard;
    }
    [System.Serializable] public class Question
    {
        public int number;
        public string question;
        public List<Option> options;
        public string answer;
    }
    [System.Serializable] public class Option
    {
        public string key;
        public string value;
    }

    public TMP_Text categoryTextUI;
    public TMP_Text questionTextUI;
    public TMP_Text[] answerTextUIs;
    public Button[] answerButtons;
    public TMP_Text timerTextUI;
    public Image timerFillBar;
    public TMP_Text questionCountTextUI;

    public float maxTimePerQuestion = 20f;
    private float currentTime;
    private bool isTimerRunning = false;
    private float lastSecondPlayed = -1f;

    public Color normalColor = Color.green;

    private string selectedCategory;
    private List<Question> selectedQuestions = new List<Question>();
    private int currentQuestionIndex = 0;
    private int pressedWrongIndex = -1;
    private bool isGameOver = false;

    public Animator animator;
    public GameObject playerModel;

    void Start()
    {
        selectedCategory = PlayerPrefs.GetString("SelectedCategory", "Geography");
        categoryTextUI.text = selectedCategory + ":";

        LoadQuestions();
        ShowQuestion();

        if (playerModel != null)
        {
            animator = GameObject.Find("Player2").GetComponent<Animator>();
        }

        if (animator != null)
        {
            animator.SetBool("pressButton", false);
            animator.Play("Idle");
        }
    }

    void Update()
    {
        if (isGameOver) return;

        // Shortcut testing keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) CheckAnswer("A", 0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) CheckAnswer("B", 1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) CheckAnswer("C", 2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) CheckAnswer("D", 3);

        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f) currentTime = 0f;

            float ratio = currentTime / maxTimePerQuestion;
            timerFillBar.fillAmount = ratio;
            timerTextUI.text = Mathf.Ceil(currentTime).ToString();

            int currentSecond = Mathf.CeilToInt(currentTime);
            if (currentSecond != Mathf.CeilToInt(lastSecondPlayed))
            {
                AkUnitySoundEngine.PostEvent("Play_Timer", gameObject);
                float invertedRTPC = maxTimePerQuestion - currentTime;
                AkUnitySoundEngine.SetRTPCValue("TimeLeft", invertedRTPC, gameObject);
                lastSecondPlayed = currentTime;
            }

            if (currentTime <= 0f)
            {
                isTimerRunning = false;
                HeartManager.Instance.LoseHeart();

                HighlightAnswers(-1);
                StartCoroutine(NextQuestionAfterDelay(1f));
            }
        }
    }

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("trivia_question_bank");
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found!");
            return;
        }

        TriviaData triviaData = JsonUtility.FromJson<TriviaData>(jsonFile.text);
        if (triviaData == null || triviaData.trivia_questions == null)
        {
            Debug.LogError("Failed to parse trivia data.");
            return;
        }

        CategoryData cat = selectedCategory switch
        {
            "Geography" => triviaData.trivia_questions.Geography,
            "Math" => triviaData.trivia_questions.Math,
            "Science" => triviaData.trivia_questions.Science,
            "History" => triviaData.trivia_questions.History,
            "Sports" => triviaData.trivia_questions.Sports,
            "Psychology" => triviaData.trivia_questions.Psychology,
            _ => null
        };

        if (cat == null)
        {
            Debug.LogError("Invalid category: " + selectedCategory);
            return;
        }

        if (cat.Beginner.Count > 0)
            selectedQuestions.Add(cat.Beginner[Random.Range(0, cat.Beginner.Count)]);
        if (cat.Intermediate.Count > 0)
            selectedQuestions.Add(cat.Intermediate[Random.Range(0, cat.Intermediate.Count)]);
        if (cat.Hard.Count > 0)
            selectedQuestions.Add(cat.Hard[Random.Range(0, cat.Hard.Count)]);
    }

    void ShowQuestion()
    {
        if (animator != null)
        {
            animator.SetBool("pressButton", false);
        }

        currentTime = maxTimePerQuestion;
        lastSecondPlayed = -1f;
        isTimerRunning = true;
        timerFillBar.fillAmount = 1f;
        timerFillBar.color = normalColor;
        timerTextUI.text = Mathf.Ceil(maxTimePerQuestion).ToString();

        ResetAnswerButtonColors();
        pressedWrongIndex = -1;

        questionCountTextUI.text = (currentQuestionIndex + 1) + "/" + selectedQuestions.Count;

        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            questionTextUI.text = "You finished all questions!";
            questionCountTextUI.text = "";
            foreach (var txt in answerTextUIs) txt.text = "";
            foreach (var btn in answerButtons) btn.interactable = false;

            isGameOver = true;
            StartCoroutine(LoadSceneAfterDelay(3f));
            return;
        }

        Question q = selectedQuestions[currentQuestionIndex];
        questionTextUI.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < q.options.Count)
            {
                string optionKey = q.options[i].key;

                TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
                buttonText.text = $"{optionKey}: {q.options[i].value}";

                answerButtons[i].onClick.RemoveAllListeners();
                string capturedKey = optionKey;
                int capturedIndex = i;
                answerButtons[i].onClick.AddListener(() => CheckAnswer(capturedKey, capturedIndex));

                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].interactable = true;
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void CheckAnswer(string selectedOption, int buttonIndex)
    {
        if (!isTimerRunning || isGameOver) return;
        isTimerRunning = false;

        if (animator != null)
        {
            animator.SetBool("pressButton", true);
            StartCoroutine(ResetPressButtonAfterDelay(0.5f));
        }

        Question currentQuestion = selectedQuestions[currentQuestionIndex];

        if (selectedOption == currentQuestion.answer)
        {
            AkUnitySoundEngine.PostEvent("Play_Correct", gameObject);
        }
        else
        {
            AkUnitySoundEngine.PostEvent("Play_Wrong", gameObject);
            HeartManager.Instance.LoseHeart();
            pressedWrongIndex = buttonIndex;
        }

        HighlightAnswers(pressedWrongIndex);
        StartCoroutine(NextQuestionAfterDelay(0.8f));
    }

    void HighlightAnswers(int wrongPressedIndex)
    {
        Question currentQuestion = selectedQuestions[currentQuestionIndex];

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (!answerButtons[i].gameObject.activeSelf) continue;

            TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            string btnTextStr = btnText.text;

            if (btnTextStr.StartsWith(currentQuestion.answer + ":"))
            {
                answerButtons[i].GetComponent<Image>().color = Color.green;
                answerButtons[i].interactable = false;
            }
            else if (i == wrongPressedIndex)
            {
                answerButtons[i].GetComponent<Image>().color = Color.red;
                answerButtons[i].interactable = false;
            }
            else
            {
                answerButtons[i].interactable = false;
            }
        }
    }

    void ResetAnswerButtonColors()
    {
        foreach (var btn in answerButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
            btn.interactable = true;
        }
    }

    IEnumerator NextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentQuestionIndex++;
        ShowQuestion();
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int currentBooth = PlayerPrefs.GetInt("CurrentBooth", 1);

        if (currentBooth >= 6)
        {
            SceneManager.LoadScene("WinScene");
            PlayerPrefs.DeleteKey("CurrentBooth");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentBooth", currentBooth + 1);
            SceneManager.LoadScene("GameScene");
        }
    }

    IEnumerator ResetPressButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animator != null)
        {
            animator.SetBool("pressButton", false);
        }
    }
}
