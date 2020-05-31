using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // TODO move victory / loss conditions here (timer / distance)
    // TODO create quiz UI and game
    // TODO control notes UI and what is on the quiz (randomize)
    // Score calculator / high scores using PlayerPrefs
    // Persistent values (i.e. timer) when changing to quiz

    // Game Control
    private int _difficulty; // -1 = easy, 0 = normal, 1 = hard
    private int _score;
    private float _targetDistance;
    private float _timeElapsed = 0;
    private float _targetTime;
    private bool _timerActive = false;
    private int _raceStatus; // 0 = active, -1 = failed, 1 = complete
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
    private float _tileOffset = 0;
    private List<GameObject> _spawnedTiles;

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
        // set target time and distance based on difficulty
        // set UI score, time, and distance
        // TODO: Automate level gen process
        SpawnTile(-1);
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile();
        SpawnTile(1);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            // Alternative: return to main menu (when ready)
        }
        GameStatus();
        UpdateUI();
    }

    private void GameStatus() {
        if (_timerActive && _raceStatus == 0) {
            _timeElapsed += Time.deltaTime;
            float remainingTime = _targetTime - _timeElapsed;
            float remainingDistance = _targetDistance - _player.distanceTravelled;
            if (remainingTime <= 0) {
                // fail state
                remainingTime = 0;
                _timerActive = false;
                _raceStatus = -1;
                _player.RaceActive(false);
            } else if (remainingDistance <= 0) {
                // race completed
                remainingDistance = 0;
                _timerActive = false;
                _raceStatus = 1;
                _player.RaceActive(false);
                // move to quiz section
            }
        }
    }

    private void UpdateUI() {
        // score, distance remaining, time remaining
    }

    private void SpawnTile(int i = 0) {
        GameObject tile;
        if (i == -1) {
            // spawn empty tile
            tile = Instantiate(_hallwayEmpty) as GameObject;
            _previousTile = -1;
            Debug.Log("Spawned tile Start");
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
        tile.transform.position = new Vector3(0f, _tileOffset, 0f);
        _tileOffset += 50f;
        _spawnedTiles.Add(tile);
    }
}
