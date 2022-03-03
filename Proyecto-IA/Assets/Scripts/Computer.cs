using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField]private bool _active = false;
    [SerializeField]private int _codeCant = 10;
    bool _hacking = false;

    void Update()
    {
        if (_active) {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        } else {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public bool Coding() {
        if (!_hacking) {
            Hacking();
            _hacking = true;
        }
        if (_codeCant <= 0) {
            Invoke("Deactivate", 0.2f);
            return true;
        } else { return false; }
    }
    private void Deactivate() {
        _active = false;
    }
    public bool Active {
        get { return _active; }
    }
    private void Hacking() {
        if (_codeCant > 0) {
            _codeCant--;
            Invoke("Hacking", 0.2f);
        } else { Invoke("Reactivate", 50f); }
    }
    private void Reactivate() {
        _codeCant = 10;
        _active = true;
        _hacking = false;
    }
}
