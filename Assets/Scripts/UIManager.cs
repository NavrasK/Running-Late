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
    private Text _coinsText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _raceCompleteText;
    [SerializeField]
    private Text _raceFailedText;

    public void UpdateTimeUI(float timeRemaining) {
        if (timeRemaining >= 0) {
            _timerText.text = timeRemaining.ToString("000.00");
        } else {
            _timerText.text = "000.00";
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

    public void UpdateScoreUI(float score) {
        _scoreText.text = "SCORE: " + score;
    }

    public void RaceCompleted() {
        _raceCompleteText.gameObject.SetActive(true);
    }

    public void RaceFailed() {
        _raceFailedText.gameObject.SetActive(true);
    }
}
