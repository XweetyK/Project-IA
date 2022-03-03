//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public abstract class Unit : MonoBehaviour
//{
//    public enum unitType {Null ,Miner, Soldier, Creeper, Bowman };
//    protected unitType _type;
//    protected float _hp;
//    protected float _speed;
//    protected float _attack;
//    protected float _attackSpeed;
//    protected float _defense;
    
//    public virtual void moveCommand(Node targetNode){
//        StopCoroutine("moveTo");
//        Node posNode = NodeSystem.Instance.findNode(transform.position);
//        List<Node> pathToTake = NodeSystem.Instance.GetComponent<Pathfinding>().findPath(posNode, targetNode);
//        StartCoroutine(moveTo(pathToTake));
//    }
    

//    public virtual void attack(){

//    }

//    public virtual void takeDamage(float damage){

//    }

//    public virtual void death(){

//    }

//    public unitType getUnitType{
//        get{ return this._type; }
//        set { this._type = value; }
        
//    }

//    //Couroutines

//    IEnumerator moveTo(List<Node> path){
//        bool arrived = false;
//        int pointer = path.Count - 1;
//        float dist = NodeSystem.Instance.closeDist;
//        Node currentTarget = path[pointer];
//        while (arrived == false){
//        if(dist > NodeSystem.Instance.nodeDistance(currentTarget, transform.position)) {
//                pointer--;
//                if (pointer < 0){
//                    arrived = true;
//                    break;
//                }
//                currentTarget = path[pointer];
//            }
//            Debug.Log(NodeSystem.Instance.nodeDistance(currentTarget, transform.position));
//            Debug.Log("Nodo: " + pointer);
//            Vector3 dir = (new Vector3(currentTarget.pos.x, currentTarget.pos.y, currentTarget.pos.z) - transform.position).normalized;
//            //rot test
//            //transform.rotation = Quaternion.LookRotation(dir);
//            transform.Translate(transform.forward * _speed * Time.deltaTime);
//            yield return null;
//        }
//        yield return null;
//    }

//    Vector3 LerpMov(Vector3 start, Vector3 end, float timeStart, float lerpTime){
//        float timeTraveled = Time.time - timeStart;
//        float progress = timeTraveled / lerpTime;

//        Vector3 result = Vector3.Lerp(start, end, progress);
//        return result;
//    }


//}
