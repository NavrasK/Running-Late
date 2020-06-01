using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private Player _player;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 4, -10);
    private bool _followActive = true;
    private Animator _animator;

    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) {
            Debug.LogError("Camera could not find player transform");
        }
        _animator = gameObject.GetComponent<Animator>();
        transform.position = _offset;
    }

    private void LateUpdate() {
        transform.position = new Vector3(0, _player.transform.position.y, 0) + _offset;
    }
}
