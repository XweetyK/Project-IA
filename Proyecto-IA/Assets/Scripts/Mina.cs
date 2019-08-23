using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : MonoBehaviour
{
    [SerializeField] private int _oreCant;

    public bool Mining() {
        _oreCant --;
        if (_oreCant == 0) {
            Invoke("OutOfGold", 0.2f);
            return true;
        } else { return false; }
    }
    private void OutOfGold() {
        Destroy(gameObject);
    }
}
