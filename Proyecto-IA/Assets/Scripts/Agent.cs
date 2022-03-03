using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    [SerializeField] Pathfinding _pathFinder;
    [SerializeField] float _speed = 3.0f;
    [SerializeField] float _rotSpeed = 2.0f;
    [SerializeField] Animator _anim;
    [SerializeField] private float _minDist;
    [SerializeField] private float _codeTime;

    public enum state { idle = 0, travel, coding, returnHome, deposit, count }
    public enum events { GetData = 0, Code, DoneCoding, ArrivedHome, Empty, ResumeCoding, count }
    private FSM _fsm;
    private bool _coding = false;
    private bool _inHome = false;

    private List<Node> _nodePath;
    Node _initNode;
    Node _destNode;
    bool _shouldMove = false;
    int _cont = 0;
    bool _startCode = false;

    Transform _home;
    GameObject[] _computer;
    GameObject _activeComputer;

    void Start() {
        _fsm = new FSM((int)state.count, (int)events.count);
        _fsm.SetRelation((int)state.idle, (int)state.travel, (int)events.GetData);
        _fsm.SetRelation((int)state.travel, (int)state.coding, (int)events.Code);
        _fsm.SetRelation((int)state.coding, (int)state.returnHome, (int)events.DoneCoding);
        _fsm.SetRelation((int)state.returnHome, (int)state.deposit, (int)events.ArrivedHome);
        _fsm.SetRelation((int)state.deposit, (int)state.idle, (int)events.Empty);
        _fsm.SetRelation((int)state.deposit, (int)state.travel, (int)events.ResumeCoding);

        _home = GameObject.FindGameObjectWithTag("Home").transform;
        _computer = GameObject.FindGameObjectsWithTag("Computer");
        Debug.Log(_home.gameObject.name);
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            GetPath();
        }
        if (_shouldMove) {
            MoveToDest();
        }
        AnimManager();

        switch (_fsm.ActualState) {
            case 0: //------------------------------- Idle
                Idle();
                break;
            case 1: //------------------------------- Ir
                GetData();
                break;
            case 2: //------------------------------- Minando
                Coding();
                break;
            case 3: //------------------------------- Traer
                GoHome();
                break;
            case 4: //------------------------------- Guardar
                Store();
                break;
        }
    }

    void GetPath() {
        _initNode = _pathFinder.GetNodeByCoord(transform.position);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
            _destNode = _pathFinder.GetNodeByCoord(hit.point);
        }
        _nodePath = _pathFinder.FindPath(_initNode, _destNode);
        if (_nodePath != null) {
            _shouldMove = true;
        }
        _cont = _nodePath.Count - 1;
    }

    void GetPath(Vector3 destination) {
        _initNode = _pathFinder.GetNodeByCoord(transform.position);
        _destNode = _pathFinder.GetNodeByCoord(destination);

        _nodePath = _pathFinder.FindPath(_initNode, _destNode);
        if (_nodePath != null) {
            _shouldMove = true;
        }
        _cont = _nodePath.Count - 1;
    }

    void MoveToDest() {
        if (transform.position != _nodePath[_cont].Pos) {
            transform.position = Vector3.MoveTowards(transform.position, _nodePath[_cont].Pos, _speed * Time.deltaTime);
            var targetRotation = Quaternion.LookRotation(_nodePath[_cont].Pos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        } else if (_cont != 0) { _cont--; }
        if (_cont == 0 && transform.position == _nodePath[_cont].Pos) {
            _shouldMove = false;
        }
    }

    void AnimManager() {
        _anim.SetBool("IsWalking", _shouldMove);
    }

    void Idle() {
        Debug.Log("Idle()");
        foreach (GameObject c in _computer) {
            if (c.GetComponent<Computer>().Active) {
                GetPath(c.transform.position);
                _activeComputer = c;
                _fsm.SendEvent((int)events.GetData);
                return;
            } else { _activeComputer = null; }
        }
    }

    void GetData() {
        Debug.Log("GetData()");
        Vector3 _dif = transform.position - _activeComputer.transform.position;
        if (Mathf.Abs(_dif.x) < _minDist && Mathf.Abs(_dif.z) < _minDist) {
            _shouldMove = false;
            _fsm.SendEvent((int)events.Code);
        }
    }

    void Coding() {
        Debug.Log("Coding()");
        if (_activeComputer != null) {
            if (_activeComputer.GetComponent<Computer>().Coding()) {
                _activeComputer = null;
                GetPath(_home.position);
                _startCode = false;
                _fsm.SendEvent((int)events.DoneCoding);
            }
        }
    }

    void GoHome() {
        Debug.Log("GoHome()");
        Vector3 _dif = transform.position - _home.position;
        if (Mathf.Abs(_dif.x) < _minDist && Mathf.Abs(_dif.z) < _minDist) {
            _fsm.SendEvent((int)events.ArrivedHome);
        }
    }
    void Store() {
        Debug.Log("Store()");
        foreach (GameObject c in _computer) {
            if (c.GetComponent<Computer>().Active) {
                GetPath(c.transform.position);
                _activeComputer = c;
                _fsm.SendEvent((int)events.ResumeCoding);
                return;
            } else { _activeComputer = null; _fsm.SendEvent((int)events.Empty); }
        }
    }

}
