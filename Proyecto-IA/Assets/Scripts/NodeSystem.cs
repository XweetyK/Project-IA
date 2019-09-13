using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSystem : Singleton<NodeSystem> {
    [SerializeField] GameObject _terrain;
    [SerializeField] int _density;
    [SerializeField] string _layerMask;
    [SerializeField] float _borderDistance;
    [SerializeField] float _maxDist;
    [SerializeField] bool _diagonal;
    public List<Node> _nodes;
    RaycastHit hit;
    BoxCollider _col;
    Vector2 _dist;
    float _height;
    float _width;
    Vector3 _startPoint;
    bool _init = false;

    void Start() {
        _nodes = new List<Node>();
        if (_terrain) {
            _col = _terrain.GetComponent<BoxCollider>();
            Mapping();
        }
    }


    void Update() {
        if (_init) {
            ObstacleUpdate();
        }
        if (Input.GetMouseButtonDown(0)) {
            Check();
        }
    }

    void Mapping() {
        _height = (_col.bounds.max.z - _borderDistance) * 2;
        _width = (_col.bounds.max.x - _borderDistance) * 2;

        Debug.Log(_height + " x " + _width);

        _dist.x = (_width / _density);
        _dist.y = (_height / _density);

        _startPoint.x = _terrain.transform.position.x - (_width / 2);
        _startPoint.y = _terrain.transform.position.y;
        _startPoint.z = _terrain.transform.position.z - (_height / 2);
        int id = 0;
        for (int i = 0; i <= _density; i++) {
            for (int j = 0; j <= _density; j++) {

                if (Physics.Raycast(new Vector3((_startPoint.x + (_dist.x * i)), (_startPoint.y + 10), (_startPoint.z + (_dist.y * j))),
                transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, LayerMask.GetMask(_layerMask))) {
                    Node _n = new Node(hit.point, id++);
                    _nodes.Add(_n);
                }
            }
        }
        Debug.Log(_nodes.Count);
        AddNeighbors();
        _init = true;
    }

    void ObstacleUpdate() {
        foreach (Node n in _nodes) {
            if (Physics.Raycast(new Vector3(n.pos.x, n.pos.y + 10, n.pos.z), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(_layerMask)) {
                    n.obstacle = false;
                } else { n.obstacle = true; }
            }
        }
    }

    private void OnDrawGizmos() {
        if (Application.isPlaying) {
            if (_nodes.Count != 0) {
                foreach (Node n in _nodes) {
                    //if (n.obstacle) {
                    //    Gizmos.color = Color.red;
                    //} 
                    if (n.selected) {
                        Gizmos.color = Color.green;
                    } else { Gizmos.color = Color.blue; }
                    Gizmos.DrawWireSphere(n.pos, 0.5f);
                }
            }
        }
    }

    public ref List<Node> nodeList {
        get { return ref _nodes; }
    }

    private void AddNeighbors() {
        foreach (var n in _nodes) {
            foreach (var n2 in _nodes) {
                if (n != n2) {
                    float dist = Vector3.Distance(n.pos, n2.pos);
                    switch (_diagonal) {
                        case true:
                            if (dist <= (_dist.x * Mathf.Sqrt(2)) + (_dist.x / 4)) {
                                Node adj = n2;
                                n.Adjacents(adj);
                            }
                            break;

                        case false:
                            if (dist <= _dist.x + (_dist.x / 4)) {
                                Node adj = n2;
                                n.Adjacents(adj);
                            }
                            break;
                    }
                }
            }
        }
    }

    private void Check() {
        foreach (var n in _nodes) {
            n.selected = false;
        }

        int r = Random.Range(0, _nodes.Count);
        Debug.Log(r);

        _nodes[r].selected = true;
        foreach (var n in _nodes[r].adjacents) {
            n.selected = true;
        }
    }
}