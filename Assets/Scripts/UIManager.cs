using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private Text _distanceText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _raceCompleteText;
    [SerializeField]
    private Text _raceFailedText;

    private void Start() {
        // TODO
    }

    private void Update() {
        // TODO
    }

    public void UpdateTimeUI(float timeRemaining) {
        if (timeRemaining >= 0) {
            _distanceText.text = "TIME: " + timeRemaining.ToString("000.00");
        } else {
            _distanceText.text = "TIME: 000.00";
        }
    }

    public void UpdateDistanceUI(float distanceRemaining) {
        if (distanceRemaining >= 0) {
            _distanceText.text = "DISTANCE REMAINING: " + distanceRemaining.ToString("000.00");
        } else {
            _distanceText.text = "DISTANCE REMAINING: 000.00";
        }
    }

    public void UpdateScoreUI(int score) {
        _scoreText.text = "SCORE: " + score;
    }

    public void RaceCompleted() {
        _raceCompleteText.gameObject.SetActive(true);
    }

    public void RaceFailed() {
        _raceFailedText.gameObject.SetActive(true);
    }
}
