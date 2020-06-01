using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // Game Control
    private int _difficulty = 0; // -1 = easy, 0 = normal, 1 = hard
    [SerializeField]
    private int _numTiles = 20;
    [SerializeField]
    private float _timePerTile = 2.5f;
    [SerializeField]
    private float _quizTime = 30f;
    private float _targetDistance;
    private float _timeElapsed = 0;
    private float _targetTime;
    private bool _timerActive = false;
    private int _raceStatus = 0; // 0 = active, -1 = failed, 1 = complete
    private int _quizStatus = -1; // -1 = waiting, 0 = active, 1 = over (complete or time out)
    private Player _player;
    private UIManager _UI;

    // Level Generation
    [SerializeField]
    private GameObject[] _hallwaySegments;
    [SerializeField]
    private GameObject _hallwayEmpty;
    [SerializeField]
    private GameObject _hallwayEnd;
    [SerializeField]
    private GameObject _tileParent;
    private int _previousTile = -1;
    private float _tileNextSpawn = 0;
    private bool _continueSpawning = true;
    private List<GameObject> _spawnedTiles;
    private float _deleteThreshold = 100f;

    // Quiz variables
    private int _totalQuestions;
    private int _currentQuestion = 0;
    private int _correctQuestions = 0;
    [SerializeField]
    private float _questionPointsValue = 1000;
    [SerializeField]
    private float _perfectQuizBonus = 500;
    private float _difficultyMultiplier;

    private void Start() {
        _spawnedTiles = new List<GameObject>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) {
            Debug.LogError("GameManager: Cannot find Player");
        }
        _UI = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UI == null) {
            Debug.LogError("GameManager: Cannot find UIManager");
        }
        SetDifficulty();
        RaceSetup();
        QuizSetup();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            // ALT: return to main menu (once created)
        }
        if (_raceStatus == 0) {
            UpdateRaceStatus();
            UpdateRaceUI();
            SpawnManager();
        } else if (_quizStatus == 0) {
            UpdateQuizStatus();
            UpdateQuizUI();
        }
    }

    /////////////
    /// SETUP ///
    /////////////

    private void SetDifficulty() {
        // TODO: get difficulty setting from PlayerPrefs
        _difficultyMultiplier = 1; // TEMP
        _totalQuestions = 5; // TEMP
        _targetDistance = 50 * _numTiles / 10;
        _targetTime = _numTiles * _timePerTile + _quizTime;
        // TODO - set difficulty level, target distance, target time, 
        // tile sets, fact types, # of facts, # of questions, skill based question level
    }

    private void RaceSetup() {
        SpawnTile(-1);
        _player.RaceActive(true);
        _timerActive = true;
    }

    private void QuizSetup() {
        // TODO
    }

    ////////////
    /// RACE ///
    ////////////

    private void UpdateRaceStatus() {
        if (_timerActive) {
            _timeElapsed += Time.deltaTime;
            float remainingTime = _targetTime - _timeElapsed;
            float remainingDistance = _targetDistance - _player.distanceTravelled;
            if (remainingTime <= 0) {
                // fail state
                remainingTime = 0;
                _timerActive = false;
                _raceStatus = -1;
                _player.RaceActive(false);
                _UI.TimeUp();
            } else if (remainingDistance <= 0) {
                // race completed
                remainingDistance = 0;
                _raceStatus = 1;
                _player.RaceActive(false);
                _quizStatus = 0;
                StartCoroutine(ActivateQuiz());
            }
        }
    }

    private void UpdateRaceUI() {
        _UI.UpdateDistanceUI(_targetDistance - _player.distanceTravelled);
        _UI.UpdateTimeUI(_targetTime - _timeElapsed);
    }

    private void SpawnManager() {
        // If the player is within 3 tiles of the next spawn location, spawn a new one
        if (_continueSpawning) {
            if (_player.transform.position.y > _tileNextSpawn - 200) {
                // If the next spawn location is the end create finish line and stop spawning
                if (_targetDistance == _tileNextSpawn / 10) {
                    _continueSpawning = false;
                    SpawnTile(1);
                    SpawnTile(-1);
                } else {
                    SpawnTile();
                }
            }
        }
        if (_player.transform.position.y > _deleteThreshold) {
            Destroy(_spawnedTiles[0]);
            _spawnedTiles.RemoveAt(0);
            _deleteThreshold += 50;
        }
    }

    private void SpawnTile(int i = 0) {
        GameObject tile;
        if (i == -1) {
            // spawn empty tile
            tile = Instantiate(_hallwayEmpty) as GameObject;
            _previousTile = -1;
            Debug.Log("Spawned tile Empty");
        } else if (i == 1) {
            // spawn end tile
            tile = Instantiate(_hallwayEnd) as GameObject;
            _previousTile = -1;
            Debug.Log("Spawned tile End");
        } else {
            // spawn random tile
            int tileIndex = Random.Range(0, _hallwaySegments.Length);
            while (tileIndex == _previousTile) {
                tileIndex = Random.Range(0, _hallwaySegments.Length);
            }
            tile = Instantiate(_hallwaySegments[tileIndex]) as GameObject;
            _previousTile = tileIndex;
            Debug.Log("Spawned tile " + tileIndex);
        }
        tile.transform.SetParent(_tileParent.transform);
        tile.transform.position = new Vector3(0f, _tileNextSpawn, 0f);
        _tileNextSpawn += 50f;
        _spawnedTiles.Add(tile);
    }

    ////////////
    /// QUIZ ///
    ////////////

    IEnumerator ActivateQuiz() {
        _UI.ActivateQuiz();
        _timerActive = false;
        while (!_UI.quizReady) {
            yield return new WaitForEndOfFrame();
        }
        _timerActive = true;
    }

    private void UpdateQuizStatus() {
        if (_timerActive) {
            _timeElapsed += Time.deltaTime;
            float remainingTime = _targetTime - _timeElapsed;
            if (remainingTime <= 0) {
                remainingTime = 0;
                _timerActive = false;
                _UI.TimeUp();
            }
            // if questions are done, move to end screen
        }
    }

    private void UpdateQuizUI() {
        _UI.UpdateTimeUI(_targetTime - _timeElapsed);
    }

    ///////////////
    /// ENDGAME ///
    ///////////////

    private int CalculateScore() {
        float finalScore = _player.raceScore;
        finalScore += _questionPointsValue * (_correctQuestions / _totalQuestions);
        if (_correctQuestions == _totalQuestions) {
            finalScore += _perfectQuizBonus;
        }
        if (_correctQuestions / _totalQuestions >= 0.5f) {
            finalScore *= 1 + (_targetTime - _timeElapsed);
        }
        finalScore *= _difficultyMultiplier;
        return Mathf.RoundToInt(finalScore);
    }
}
