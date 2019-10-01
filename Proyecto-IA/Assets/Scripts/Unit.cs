using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public enum unitType {Null ,Miner, Soldier, Creeper, Bowman };
    unitType type;
    protected float _hp;
    protected float _speed;
    protected float _attack;
    protected float _attackSpeed;
    protected float _defense;
    
    public virtual void moveCommand(Node targetNode){
        StopCoroutine("moveTo");
        Node posNode = NodeSystem.Instance.findNode(transform.position);
        StartCoroutine("moveTo", NodeSystem.Instance.GetComponent<Pathfinding>().findPath(posNode, targetNode));
    }
    

    public virtual void attack(){

    }

    public virtual void takeDamage(float damage){

    }

    public virtual void death(){

    }

    public unitType getUnitType{
        get{ return this.type; }
        set { this.type = value; }
        
    }

    //Couroutines

    IEnumerator moveTo(List<Node> path){
        bool arrived = false;
        int pointer = 0;
        float dist = NodeSystem.Instance.closeDist;
        Node currentTarget = path[pointer];
        while (arrived == false){

            if(dist < NodeSystem.Instance.nodeDistance(currentTarget, transform.position)) {
                pointer++;
                currentTarget = path[pointer];
                transform.Translate(new Vector3(currentTarget.pos.x, transform.position.y, currentTarget.pos.z)*_speed);
            }
            yield return null;
        }
        yield return null;
    }

    Vector3 LerpMov(Vector3 start, Vector3 end, float timeStart, float lerpTime){
        float timeTraveled = Time.time - timeStart;
        float progress = timeTraveled / lerpTime;

        Vector3 result = Vector3.Lerp(start, end, progress);
        return result;
    }


}
