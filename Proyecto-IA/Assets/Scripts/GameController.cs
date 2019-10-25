using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool _selecUnit = false;
    private Unit _unit;
    private float _timing=0;

    void Update() {
        MoveUnit();
        _timing += Time.deltaTime;
    }

    public void MoveUnit() {
        if (Input.GetMouseButtonDown(0) && !_selecUnit) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                Unit[] _tUnits = GameObject.FindObjectsOfType<Unit>();
                foreach (Unit u in _tUnits) {
                    if (Vector3.Distance(u.transform.position, hit.point) < 2) {
                        _unit = u;
                        _selecUnit = true;
                        _timing = 0;
                        Debug.LogWarning("unit selected!");
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && _selecUnit && _timing>2) {
            RaycastHit hit;
            Node init, dest;
            
            init=NodeSystem.Instance.findNode(_unit.transform.position);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                dest = NodeSystem.Instance.findNode(hit.point);
                Debug.LogWarning("dest selected!");
                List<Node> t = Pathfinding.Instance.findPath(init, dest);
                if ( t != null) {
                    _unit.Path = t;
                    Debug.LogWarning("path assigned!");
                }
            }
            _unit.moveCommand();
            Debug.LogWarning("moving!");
        }
        if (Input.GetMouseButtonDown(1) && _selecUnit) {
            Debug.LogWarning("Release Unit!");
            _unit = null;
            _selecUnit = false;
        }
    }
}
