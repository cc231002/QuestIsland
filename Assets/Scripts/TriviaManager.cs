using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Add this for Button

public class TriviaManager : MonoBehaviour
{
    // JSON data classes
    [System.Serializable]
    public class TriviaData
    {
        public TriviaCategories trivia_questions;
    }

    [System.Serializable]
    public class TriviaCategories
    {
        public GeographyCategory Geography;
    }

    [System.Serializable]
    public class GeographyCategory
    {
        public List<Question> Beginner;
        public List<Question> Intermediate;
        public List<Question> Hard;
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

    // UI References
    public TMP_Text questionTextUI;
    public TMP_Text[] answerTextUIs;

    public Button[] answerButtons; // Buttons A, B, C, D

    private List<Question> selectedQuestions = new List<Question>();
    private int currentQuestionIndex = 0;

    void Start()
    {
        LoadQuestions();
        ShowQuestion();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) CheckAnswer("A");
        if (Input.GetKeyDown(KeyCode.Alpha2)) CheckAnswer("B");
        if (Input.GetKeyDown(KeyCode.Alpha3)) CheckAnswer("C");
        if (Input.GetKeyDown(KeyCode.Alpha4)) CheckAnswer("D");
    }

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("trivia_question_bank");
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found in Resources!");
            return;
        }

        // Wrap the root object manually
        string wrappedJson = jsonFile.text;
        TriviaData triviaData = JsonUtility.FromJson<TriviaData>(wrappedJson);

        if (triviaData == null || triviaData.trivia_questions == null)
        {
            Debug.LogError("Failed to parse trivia data.");
            return;
        }

        GeographyCategory geo = triviaData.trivia_questions.Geography;

        // Randomly select one question per difficulty
        if (geo.Beginner.Count > 0)
            selectedQuestions.Add(geo.Beginner[Random.Range(0, geo.Beginner.Count)]);
        if (geo.Intermediate.Count > 0)
            selectedQuestions.Add(geo.Intermediate[Random.Range(0, geo.Intermediate.Count)]);
        if (geo.Hard.Count > 0)
            selectedQuestions.Add(geo.Hard[Random.Range(0, geo.Hard.Count)]);
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            questionTextUI.text = "You finished all questions!";
            foreach (var txt in answerTextUIs) txt.text = "";
            return;
        }

        Question q = selectedQuestions[currentQuestionIndex];
        questionTextUI.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
{
    if (i < q.options.Count)
    {
        string optionKey = q.options[i].key;

        // Set the text of the button
        TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
        buttonText.text = $"{optionKey}: {q.options[i].value}";

        // Clear previous listeners to avoid stacking
        answerButtons[i].onClick.RemoveAllListeners();

        // Add a listener for this specific option
        answerButtons[i].onClick.AddListener(() => CheckAnswer(optionKey));
    }
    else
    {
        answerButtons[i].gameObject.SetActive(false);
    }
}

    }

    void CheckAnswer(string selectedOption)
    {
        if (currentQuestionIndex >= selectedQuestions.Count) return;

        Question currentQuestion = selectedQuestions[currentQuestionIndex];

        if (selectedOption == currentQuestion.answer)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Wrong!");
        }

        currentQuestionIndex++;
        ShowQuestion();
    }
}
