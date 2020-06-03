using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    // General UI variables
    [SerializeField]
    private Text _timeUpText;
    [SerializeField]
    private Text _victoryText;
    [SerializeField]
    private Text _loserText;
    private bool _isRace = true;
    public int quizReady = -1;
    private Animator _mainCameraAnimator;
    private Animator _notesCameraAnimator;

    // Race UI variables
    [SerializeField]
    private GameObject _raceUI;
    private Animator _raceUIAnimator;
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _distanceText;
    [SerializeField]
    private Text _coinsText;

    // Quiz UI variables
    [SerializeField]
    private GameObject _quizUI;
    [SerializeField]
    private Text _quizTimerText;
    [SerializeField]
    private Text _questionCounter;
    [SerializeField]
    private Text _quizScoreText;
    [SerializeField]
    private Text _questionDisplay;
    [SerializeField]
    private Text _option0;
    [SerializeField]
    private Text _option1;
    [SerializeField]
    private Text _option2;
    [SerializeField]
    private Text _option3;

    private void Start() {
        _raceUIAnimator = _raceUI.GetComponent<Animator>();
        if (_raceUIAnimator == null) {
            Debug.LogError("UIManager: Cannot find Animator on RaceUI");
        }
        _mainCameraAnimator = GameObject.Find("CameraRig").GetComponent<Animator>();
        if (_mainCameraAnimator == null) {
            Debug.LogError("UIManager: Cannot find Animator on CameraRig");
        }
        _notesCameraAnimator = GameObject.Find("NotesCamera").GetComponent<Animator>();
        if (_notesCameraAnimator == null) {
            Debug.LogError("UIManager: Cannot find Animator on NotesCamera");
        }
        _raceUI.gameObject.SetActive(true);
        _quizUI.gameObject.SetActive(false);
    }

    public void UpdateTimeUI(float timeRemaining) {
        Text timer;
        if (_isRace) {
            timer = _timerText;
        } else {
            timer = _quizTimerText;
        }
        if (timeRemaining <= 10) {
            timer.color = Color.red;
        }
        if (timeRemaining <= 0) {
            timeRemaining = 0;
        }
        timer.text = timeRemaining.ToString("000.00");
    }

    public void UpdateDistanceUI(float distanceRemaining) {
        if (distanceRemaining >= 0) {
            _distanceText.text = distanceRemaining.ToString("000.00");
        } else {
            _distanceText.text = "000.00";
        }
    }

    public void UpdateCoinsUI(int coins) {
        _coinsText.text = coins.ToString();
    }

    public void TimeUp() {
        _timeUpText.gameObject.SetActive(true);
        // TODO transition to score calculation screen
    }

    public void GameOver(bool victory) {
        if (victory) {
            _victoryText.gameObject.SetActive(true);
        } else {
            _loserText.gameObject.SetActive(true);
        }
        // TODO transition to score calculation screen
    }

    public void ActivateQuiz() {
        _isRace = false;
        StartCoroutine(QuizTransition());
    }

    IEnumerator QuizTransition() {
        _mainCameraAnimator.SetTrigger("RaceOver");
        _notesCameraAnimator.SetTrigger("RaceOver");
        _raceUIAnimator.SetTrigger("RaceOver");
        yield return new WaitForSeconds(0.8f);
        _raceUI.gameObject.SetActive(false);
        // TODO: quiz UI entrance animation (triggered automatically)
        _quizUI.gameObject.SetActive(true);
        quizReady = 0;
        yield return new WaitForSeconds(1f);
        quizReady = 1;
    }

    public void UpdateQuizUI(int index, int numQ, Question q) {
        _questionCounter.text = "Question " + index + " / " + numQ;
        _questionDisplay.text = q.q;
        _option0.text = q.a0;
        _option1.text = q.a1;
        _option2.text = q.a2;
        _option3.text = q.a3;
    }

    public void UpdateQuizScore(int score) {
        _quizScoreText.text = "SCORE: " + score;
    }
}
