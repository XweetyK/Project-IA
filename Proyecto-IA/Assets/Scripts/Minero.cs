using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minero : MonoBehaviour {

    public enum state { idle = 0, travel, mining, returnHome, deposit, count }
    public enum events { GoMine = 0, Mine, DoneMining, ArrivedHome, Empty, ResumeMine , count }
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
        _fsm = new FSM((int)state.count,(int) events.count);
        _fsm.SetRelation((int)state.idle, (int)state.travel, (int)events.GoMine);
        _fsm.SetRelation((int)state.travel, (int)state.mining, (int)events.Mine);
        _fsm.SetRelation((int)state.mining, (int)state.returnHome, (int)events.DoneMining);
        _fsm.SetRelation((int)state.returnHome, (int)state.deposit, (int)events.ArrivedHome);
        _fsm.SetRelation((int)state.deposit, (int)state.idle, (int)events.Empty);
        _fsm.SetRelation((int)state.deposit, (int)state.travel, (int)events.ResumeMine);

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
        //Debug.Log("Idle()");
        if (_cantMinas != 0) {
            _fsm.SendEvent((int)events.GoMine);
        }
    }

    void GoMine() {
        //Debug.Log("GoMine()");
        _nma.SetDestination(_minas[_cantMinas-1].transform.position);
        Vector3 _dif = transform.position - _minas[_cantMinas-1].transform.position;
        //Debug.Log(Mathf.Abs(_dif.x));
        if (Mathf.Abs(_dif.x) < _minDist && Mathf.Abs(_dif.z) < _minDist) {
            _fsm.SendEvent((int)events.Mine);
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
        //Debug.Log("Minando()");
        if (_minas[_cantMinas-1].Mining() == false) {
            Invoke("Mining", _mineTime);
        } else {
            _cantMinas--;
            _fsm.SendEvent(2);
        }
    }

    void GoHome() {
        //Debug.Log("GoHome()");
        _nma.SetDestination(_house.position);
        Vector3 _dif = transform.position -_house.position;
        if (Mathf.Abs(_dif.x) < _minDist && Mathf.Abs(_dif.z) < _minDist) {
            _fsm.SendEvent((int)events.ArrivedHome);
        }
    }
    void Store() {
        //Debug.Log("Store()");
        if (!_inHome) {
            Invoke("StoreOre", 0.5f);
            _inHome = true;
            _mining = false;
        }
    }

    void StoreOre() {
        //Debug.Log("StoreOre()");
        if (_cantMinas != 0) {
            _fsm.SendEvent((int)events.ResumeMine);
        } else { _fsm.SendEvent((int)events.Empty); }
    }
}
