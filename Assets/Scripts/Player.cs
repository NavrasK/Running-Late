﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour {
    // Movement variables
    private float _hSpeed = 13f;
    private float _speed = 15f;
    private float _accel = 2.5f;
    private float _maxSpeed = 30f;
    private float _minSpeed = 12f;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    // Damage variables
    private SpriteRenderer _sprite;
    private float _iFrames = 0.5f;
    private bool _invincible = false;

    // Score calculations
    public int coins = 0;
    public float raceScore = 0;

    // Game control
    private UIManager _UI;
    public float distanceTravelled = 0;
    private bool _raceActive = false;

    private void Start() {
        transform.position = new Vector3(0, 0, 0);
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _UI = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UI == null) {
            Debug.LogError("Player: Cannot find UIManager");
        }
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
            _speed -= 10f * _accel * Time.fixedDeltaTime;
            if (_speed <= 0) {
                _speed = 0;
            }
            _movement.x = 0;
            _movement.y = _speed;
            Vector2 moveTarget = _rb.position + _movement * Time.fixedDeltaTime;
            moveTarget.x = Mathf.Clamp(moveTarget.x, -5f, 5f);
            _rb.MovePosition(moveTarget);
        }
    }

    public void SetDifficulty(int difficulty) {
        if (difficulty == -1) {
            _minSpeed = 15f;
        } else if (difficulty == 1) {
            _minSpeed = 6f;
            _maxSpeed = 40f;
            _accel = 2.75f;
        }
    }

    public void RaceActive(bool status) {
        _raceActive = status;
    }

    public void AddCoins(int val) {
        coins += val;
        // TODO: score multiplier system
        raceScore = coins; //TEMP
        _UI.UpdateCoinsUI(coins);
    }

    public void Damaged(float slowdown) {
        if (!_invincible) {
            _speed = Mathf.Clamp(_speed - slowdown, _minSpeed, _maxSpeed);
            StartCoroutine(DamageSequence());
        }
    }

    IEnumerator DamageSequence() {
        _invincible = true;
        CameraShaker.Instance.ShakeOnce(5f, 2f, 0f, 1f);
        // TODO replace with damage animation
        Color col = _sprite.color;
        col.a = 0.5f;
        _sprite.color = col;
        yield return new WaitForSeconds(_iFrames);
        col.a = 1f;
        _sprite.color = col;
        _invincible = false;
    }
}
