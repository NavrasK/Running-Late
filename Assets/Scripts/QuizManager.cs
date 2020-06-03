using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {
    private int _status = -1; // -1 = waiting, 0 = active, 1 = completed
    private UIManager _UI;
    [SerializeField]
    private GameObject _notesPage;
    [SerializeField]
    private GameObject _notePrefab;
    [SerializeField]
    private List<string> _notes;
    [SerializeField]
    private List<Question> _questions;
    private int _numQuestions;
    private int _questionIndex;
    private Question _currentQuestion;
    public int _quizScore;

    private void Start() {
        _UI = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UI == null) {
            Debug.LogError("QuizManager: Cannot find UIManager");
        }
        _notes = new List<string>();
        _questions = new List<Question>();
    }

    public void GenerateNotesPage(int numNotes) {
        int offset;
        if (numNotes > 5) {
            offset = 50;
        } else {
            offset = 60;
        }
        for (int i = 0; i < numNotes; i++) {
            GameObject note = Instantiate(_notePrefab, _notesPage.transform);
            float notePosition = 113 - (i * offset);
            note.transform.localPosition = new Vector3(0, notePosition, 0);
            Text noteText = note.GetComponent<Text>();
            string noteContents = GenerateNote();
            _notes.Add(noteContents);
            note.name = "Note " + (i + 1).ToString() + ": " + noteContents;
            noteText.text = noteContents;
            if (numNotes <= 5) {
                noteText.fontSize = 25;
            }
            Debug.Log("Generated note " + i);
        }
    }

    private string GenerateNote() {
        return "> The traitor is BLAXABOT!";
    }

    public void GenerateQuiz(int numQ) {
        // difficulty level can control question / answer sets and number of questions / notes
        _numQuestions = numQ;
        for (int i = 0; i < _numQuestions; i++) {
            _questions.Add(GenerateQuestion(i));
        }
        ShuffleQuestions();
    }

    private void ShuffleQuestions() {
        for (int i = 0; i < _questions.Count; i++) {
            Question temp = _questions[i];
            int randIndex = Random.Range(i, _questions.Count);
            _questions[i] = _questions[randIndex];
            _questions[randIndex] = temp;
        }
    }

    public Question GenerateQuestion(int i) {
        // TODO generate questions
        int r = Random.Range(1, 5);
        if (r == 1) {
            return new Question("EXAMPLE QUESTION " + i, "CORRECT 1", "WRONG 2", "WRONG 3", "WRONG 4", 0);
        } else if (r == 2) {
            return new Question("EXAMPLE QUESTION " + i, "WRONG 1", "CORRECT 2", "WRONG 3", "WRONG 4", 1);
        } else if (r == 3) {
            return new Question("EXAMPLE QUESTION " + i, "WRONG 1", "WRONG 2", "CORRECT 3", "WRONG 4", 2);
        } else {
            return new Question("EXAMPLE QUESTION " + i, "WRONG 1", "WRONG 2", "WRONG 3", "CORRECT 4", 3);
        }
    }

    public int GetStatus() {
        return _status;
    }

    public void StartQuiz() {
        _questionIndex = 0;
        AskQuestion(_questionIndex);
    }

    private void AskQuestion(int i) {
        _currentQuestion = _questions[i];
        _UI.UpdateQuizUI(_questionIndex + 1, _numQuestions, _currentQuestion);
    }

    public void ButtonPressed(int index) {
        if (_currentQuestion.ans == index) {
            _quizScore += 1;
            _UI.UpdateQuizScore(_quizScore);
        }
        _questionIndex += 1;
        if (_questionIndex >= _questions.Count) {
            _status = 1;
            // quiz over
        } else {
            AskQuestion(_questionIndex);
        }
    }
}
