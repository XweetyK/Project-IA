using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minero : MonoBehaviour {
    [SerializeField] private float _minDist;
    [SerializeField] private float _mineTime;
    [SerializeField] private Transform _house;
    private FSM _fsm;
    private Mina[] _minas;
    private NavMeshAgent _nma;
    private bool _mining = false;
    private bool _inHome = false;
    private int _cantMinas;

    void Start() {
        _fsm = new FSM(5, 6);
        _fsm.SetRelation(0, 1, 0);
        _fsm.SetRelation(1, 2, 1);
        _fsm.SetRelation(2, 3, 2);
        _fsm.SetRelation(3, 4, 3);
        _fsm.SetRelation(4, 0, 4);
        _fsm.SetRelation(4, 1, 5);

        _minas = FindObjectsOfType<Mina>();
        _nma = GetComponent<NavMeshAgent>();
        _cantMinas = _minas.Length;
    }

    void Update() {
        switch (_fsm.ActualState) {
            case 0: //------------------------------- Idle
                Idle();
                break;
            case 1: //------------------------------- Ir
                GoMine();
                break;
            case 2: //------------------------------- Minando
                Mine();
                break;
            case 3: //------------------------------- Traer
                GoHome();
                break;
            case 4: //------------------------------- Guardar
                Store();
                break;
        }
    }

    void Idle() {
        Debug.Log("Idle()");
        if (_cantMinas != 0) {
            _fsm.SendEvent(0);
        }
    }

    void GoMine() {
        Debug.Log("GoMine()");
        _nma.SetDestination(_minas[_cantMinas-1].transform.position);
        Vector3 _dif = transform.position - _minas[_cantMinas-1].transform.position;
        Debug.Log(Mathf.Abs(_dif.x));
        if (Mathf.Abs(_dif.x) < _minDist && Mathf.Abs(_dif.z) < _minDist) {
            _fsm.SendEvent(1);
        }
    }

    void Mine() {
        if (!_mining) {
            Invoke("Mining", _mineTime);
            _inHome = false;
            _mining = true;
        }
    }

    void Mining() {
        Debug.Log("Minando()");
        if (_minas[_cantMinas-1].Mining() == false) {
            Invoke("Mining", _mineTime);
        } else {
            _cantMinas--;
            _fsm.SendEvent(2);
        }
    }

    void GoHome() {
        Debug.Log("GoHome()");
        _nma.SetDestination(_house.position);
        Vector3 _dif = transform.position -_house.position;
        if (Mathf.Abs(_dif.x) < _minDist && Mathf.Abs(_dif.z) < _minDist) {
            _fsm.SendEvent(3);
        }
    }
    void Store() {
        Debug.Log("Store()");
        if (!_inHome) {
            Debug.Log("Invoke()");

            Invoke("StoreOre", 0.5f);
            _inHome = true;
            _mining = false;
        }
    }

    void StoreOre() {
        Debug.Log("StoreOre()");
        if (_cantMinas != 0) {
            _fsm.SendEvent(5);
        } else { _fsm.SendEvent(4); }
    }
}
