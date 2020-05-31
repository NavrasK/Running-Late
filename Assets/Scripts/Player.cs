using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour {
    // Movement variables
    private float _hSpeed = 13f;
    private float _speed = 15f;
    private float _accel = 1.75f;
    private float _maxSpeed = 30f;
    private float _minSpeed = 10f;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    // Damage variables
    private SpriteRenderer _sprite;
    private float _iFrames = 0.5f;
    private bool _invincible = false;

    // Game control
    public int raceScore = 0;
    public float distanceTravelled = 0;
    private bool _raceActive = false;

    private void Start() {
        transform.position = new Vector3(0, 0, 0);
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if (_raceActive) {
            // Normal movement
            _movement.x = Input.GetAxisRaw("Horizontal") * _hSpeed;
            if (_speed < _maxSpeed) {
                _speed += _accel * Time.fixedDeltaTime;
            }
            _speed = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);
            _movement.y = _speed;
            Vector2 moveTarget = _rb.position + _movement * Time.fixedDeltaTime;
            moveTarget.x = Mathf.Clamp(moveTarget.x, -5f, 5f);
            _rb.MovePosition(moveTarget);
            distanceTravelled = transform.position.y / 10;
        } else {
            // Race is inactive (failed or completed): drift to stop
            _speed -= 2f * _accel * Time.fixedDeltaTime;
            if (_speed <= 0) {
                _speed = 0;
            }
            _movement.x = 0;
            _movement.y = _speed;
            Vector2 moveTarget = _rb.position + _movement * Time.fixedDeltaTime;
            moveTarget.x = Mathf.Clamp(moveTarget.x, -5f, 5f);
            _rb.MovePosition(moveTarget);
            // explosion animation if you failed?
        }
    }

    public void RaceActive(bool status) {
        _raceActive = status;
    }

    public void AddScore(int val) {
        raceScore += val;
    }

    public void Damaged(float slowdown) {
        if (!_invincible) {
            _speed = Mathf.Clamp(_speed - slowdown, _minSpeed, _maxSpeed);
            StartCoroutine(Invincible());
        }
        CameraShaker.Instance.ShakeOnce(5f, 2f, 0f, 1f);
    }

    IEnumerator Invincible() {
        _invincible = true;
        // TODO damaged animation
        Color col = _sprite.color;
        col.a = 0.5f;
        _sprite.color = col;
        yield return new WaitForSeconds(_iFrames);
        col.a = 1f;
        _sprite.color = col;
        _invincible = false;
    }
}
