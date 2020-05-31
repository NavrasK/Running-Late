using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
    [SerializeField]
    private int _coinValue = 1;

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
                player.AddCoins(_coinValue);
            }
            Destroy(this.gameObject);
        }
    }
}
