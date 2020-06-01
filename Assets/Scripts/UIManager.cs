using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    // General UI variables
    [SerializeField]
    private Text _timeUpText;
    private bool _isRace = true;
    public bool quizReady = false;
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
    private Text _questionDisplay;
    [SerializeField]
    private Text _option1;
    [SerializeField]
    private Text _option2;
    [SerializeField]
    private Text _option3;
    [SerializeField]
    private Text _option4;

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
        if (_isRace) {
            if (timeRemaining >= 0) {
                if (timeRemaining <= 10) {
                    _timerText.color = Color.red;
                }
                _timerText.text = timeRemaining.ToString("000.00");
            } else {
                _timerText.text = "000.00";
            }
        } else {
            if (timeRemaining >= 0) {
                if (timeRemaining <= 10) {
                    _quizTimerText.color = Color.red;
                }
                _quizTimerText.text = timeRemaining.ToString("000.00");
            } else {
                _quizTimerText.text = "000.00";
            }
        }
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
        _quizUI.gameObject.SetActive(true); // have entrance animation trigger automatically
        yield return new WaitForSeconds(1f);
        quizReady = true;
    }
}
