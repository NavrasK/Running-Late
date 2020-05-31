using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private Transform _target;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 4, -10);

    void Start() {
        _target = GameObject.Find("Player").GetComponent<Transform>();
        if (_target == null) {
            Debug.LogError("Camera could not find player transform");
        }
        transform.position = _offset;
    }

    void LateUpdate() {
        transform.position = new Vector3(0, _target.position.y, 0) + _offset;
    }
}
