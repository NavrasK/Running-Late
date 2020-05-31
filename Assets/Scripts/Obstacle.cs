using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField]
    private float _slowdown = 10f;

    private void Start() {
        // TODO
    }

    private void Update() {
        // TODO
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                player.Damaged(_slowdown);
            }
        }
    }
}
