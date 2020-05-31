using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {
    private Transform _target;
    private float _smoothSpeed = 0.3f;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, -2, -10);

    void Start() {
        _target = GameObject.Find("Player").GetComponent<Transform>();
        if (_target == null) {
            Debug.LogError("Camera could not find player transform");
        }
    }

    void LateUpdate() {
        Vector3 targetPos = _target.position + _offset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _smoothSpeed);
        transform.position = smoothPosition;
        transform.LookAt(_target);
    }
}
