using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelCompletePoint : Singleton<LevelCompletePoint> {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerCollision.Instance.complete();
        }
    }
    
}
