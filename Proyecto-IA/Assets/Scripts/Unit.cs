using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public enum unitType {Null ,Miner, Soldier, Creeper, Bowman };
    protected unitType _type;
    protected float _hp;
    protected float _speed;
    protected float _attack;
    protected float _attackSpeed;
    protected float _defense;
    protected List<Node> _nPath;
    bool move = false;
    bool arrived = false;
    int actualNode = 1;

    public virtual void moveCommand(){
        //StopCoroutine("moveTo");
        //StartCoroutine(moveTo());
        actualNode = 1;
        arrived = false;
        move = true;
    }
    

    public virtual void attack(){

    }

    public virtual void takeDamage(float damage){

    }

    public virtual void death(){

    }

    public unitType getUnitType{
        get{ return this._type; }
        set { this._type = value; }
        
    }
    protected void Update() {
        if (move) {
            if (!arrived) {
                if (Vector3.Distance(gameObject.transform.position, _nPath.ToArray()[actualNode].pos) > 0) {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _nPath.ToArray()[actualNode].pos, _speed*Time.deltaTime);
                }
                else if (actualNode != _nPath.Count - 1) {
                    actualNode++;
                } 
                else {
                    arrived = true;
                    move = false;
                }
            }
        }
    }
    //Couroutines
    //IEnumerator moveTo(){
    //    bool arrived = false;
    //    int actualNode = 0;
    //    Debug.Log("Start move");
    //    if (!arrived) {
    //        while (Vector3.Distance(gameObject.transform.position, _nPath[actualNode].pos) > 1) {
    //            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _nPath[actualNode].pos, _speed);
    //            Debug.Log("Node" + actualNode);
    //            Debug.Log(gameObject.transform.position);
    //        }
    //        if (actualNode != _nPath.Count - 1) {
    //            actualNode++;
    //        } else {
    //            arrived = true;
    //            Debug.Log("Nice!");
    //        }
    //    }
    //    //int pointer = path.Count - 1;
    //    //float dist = NodeSystem.Instance.closeDist;
    //    //Node currentTarget = path[pointer];
    //    //while (arrived == false) {
    //    //    if (dist > NodeSystem.Instance.nodeDistance(currentTarget, transform.position)) {
    //    //        pointer--;
    //    //        if (pointer < 0) {
    //    //            arrived = true;
    //    //            break;
    //    //        }
    //    //        currentTarget = path[pointer];
    //    //    }
    //    //    Debug.Log(NodeSystem.Instance.nodeDistance(currentTarget, transform.position));
    //    //    Debug.Log("Nodo: " + pointer);
    //    //    Vector3 dir = (new Vector3(currentTarget.pos.x, currentTarget.pos.y, currentTarget.pos.z) - transform.position).normalized;
    //    //    //rot test
    //    //    //transform.rotation = Quaternion.LookRotation(dir);
    //    //    transform.Translate(transform.forward * _speed * Time.deltaTime);
    //    //    yield return null;
    //    //}
    //    yield return null;
    //}

    Vector3 LerpMov(Vector3 start, Vector3 end, float timeStart, float lerpTime){
        float timeTraveled = Time.time - timeStart;
        float progress = timeTraveled / lerpTime;

        Vector3 result = Vector3.Lerp(start, end, progress);
        return result;
    }

    public List<Node> Path { set { _nPath = value; } }
}
