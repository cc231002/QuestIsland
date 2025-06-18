using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TriviaManager : MonoBehaviour
{
    // --- JSON Data classes ---
    [System.Serializable]
    public class TriviaData { public TriviaCategories trivia_questions; }
    [System.Serializable]
    public class TriviaCategories
    {
        public CategoryData Geography, Math, Science, History, Sports, Psychology;
    }
    [System.Serializable]
    public class CategoryData
    {
        public List<Question> Beginner, Intermediate, Hard;
    }
    [System.Serializable]
    public class Question
    {
        public int number;
        public string question;
        public List<Option> options;
        public string answer;
    }
    [System.Serializable]
    public class Option
    {
        public string key;
        public string value;
    }

    // --- UI Elements ---
    public TMP_Text categoryTextUI;     // shows the category name
    public TMP_Text questionTextUI;     // shows the question text
    public TMP_Text[] answerTextUIs;    // texts for answers
    public Button[] answerButtons;      // buttons for answers
    public TMP_Text timerTextUI;        // timer countdown text
    public Image timerFillBar;          // timer fill bar image

    public TMP_Text questionCountTextUI; // NEW: shows question number like 1/3

    // --- Timer stuff ---
    public float maxTimePerQuestion = 20f;
    private float currentTime;
    private bool isTimerRunning = false;

    public Color normalColor = Color.green; // timer bar normal color

    // --- Game data ---
    private string selectedCategory;
    private List<Question> selectedQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    // NEW: keep track of which button was pressed wrong
    private int pressedWrongIndex = -1;

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
        animator.SetBool("pressButton", false);  // Make sure it starts false
    }
    }

    void Update()
    {
        // Keyboard shortcuts to answer for quick testing
        if (Input.GetKeyDown(KeyCode.Alpha1)) CheckAnswer("A", 0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) CheckAnswer("B", 1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) CheckAnswer("C", 2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) CheckAnswer("D", 3);

        // Timer countdown
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f) currentTime = 0f;

            float ratio = currentTime / maxTimePerQuestion;
            timerFillBar.fillAmount = ratio;
            timerTextUI.text = Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0f)
            {
                isTimerRunning = false;
                Debug.Log("Time's up!");
                HeartManager.Instance.LoseHeart();

                // Highlight correct answer when time runs out
                HighlightAnswers(-1); // means no answer was pressed

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

        // Only pick 3 questions max (one from each difficulty if available)
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

        // Reset timer and visuals
        currentTime = maxTimePerQuestion;
        isTimerRunning = true;
        timerFillBar.fillAmount = 1f;
        timerFillBar.color = normalColor;
        timerTextUI.text = Mathf.Ceil(maxTimePerQuestion).ToString();

     

        // Reset any button colors back to normal and interactable
        ResetAnswerButtonColors();
        pressedWrongIndex = -1;

        // Show question count like "1/3"
        questionCountTextUI.text = currentQuestionIndex + 1 + "/" + selectedQuestions.Count;

        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            questionTextUI.text = "You finished all questions!";
        questionCountTextUI.text = ""; // for not showing 4/3 angezeigt wird

        foreach (var txt in answerTextUIs)
            txt.text = "";

        foreach (var btn in answerButtons)
            btn.interactable = false;

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

                // The trick to avoid wrong closure problem: capture local var
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

    // Modified CheckAnswer to know which button index was clicked
    void CheckAnswer(string selectedOption, int buttonIndex)
    {
        if (!isTimerRunning) return;

        isTimerRunning = false;

        if (animator != null)
{
    animator.SetBool("pressButton", true);
    StartCoroutine(ResetPressButtonAfterDelay(0.5f));
}

        

        Question currentQuestion = selectedQuestions[currentQuestionIndex];

        if (selectedOption == currentQuestion.answer)
        {
            Debug.Log("Correct!");
            AkUnitySoundEngine.PostEvent("Play_Correct", gameObject);
        }
        else
        {
            Debug.Log("Wrong!");
            AkUnitySoundEngine.PostEvent("Play_Wrong", gameObject);
            HeartManager.Instance.LoseHeart();
            pressedWrongIndex = buttonIndex; // remember wrong pressed button index
        }

    

        // Show color feedback
        HighlightAnswers(pressedWrongIndex);

        StartCoroutine(NextQuestionAfterDelay(0.8f));
    }

    // This highlights the right answer green, and wrong pressed red
    void HighlightAnswers(int wrongPressedIndex)
    {
        Question currentQuestion = selectedQuestions[currentQuestionIndex];

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (!answerButtons[i].gameObject.activeSelf) continue;

            TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            string btnTextStr = btnText.text;

            // check if this button is the correct answer
            if (btnTextStr.StartsWith(currentQuestion.answer + ":"))
            {
                // green background color for correct answer
                answerButtons[i].GetComponent<Image>().color = Color.green;
                answerButtons[i].interactable = false;
            }
            else if (i == wrongPressedIndex)
            {
                // red background for wrong pressed answer
                answerButtons[i].GetComponent<Image>().color = Color.red;
                answerButtons[i].interactable = false;
            }
            else
            {
                // disable interaction for other buttons
                answerButtons[i].interactable = false;
            }
        }
    }

    void ResetAnswerButtonColors()
    {
        foreach (var btn in answerButtons)
        {
            btn.GetComponent<Image>().color = Color.white; // reset to white background
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
        SceneManager.LoadScene("GameScene");
    }



IEnumerator ResetPressButtonAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    animator.SetBool("pressButton", false);
}


}
