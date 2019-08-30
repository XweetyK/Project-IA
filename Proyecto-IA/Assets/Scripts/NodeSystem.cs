using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSystem : MonoBehaviour
{
    Node _nodes;
    [SerializeField]GameObject _terrain;
    [SerializeField] float density = 0.1f;
    bool isInit = false;

    //MapLenght
    Vector3 pos;
    float height = 0;
    float width = 0;
    Vector3 center;

    //NodeInfo
    int nodeCant;
    void Start()
    {
        if (_terrain){
            init();
        }
    }

    void init(){
        isInit = true;
        pos = _terrain.transform.position;
        BoxCollider boxC = _terrain.GetComponent<BoxCollider>();
        center = boxC.bounds.center;
        width = boxC.bounds.max.z - boxC.bounds.min.z;
        height = boxC.bounds.max.x - boxC.bounds.min.x;
        Debug.Log("height: " + height + " Width:" + width);

    }

    void createNode(float posX, float posZ){
        Node node = new Node(new Vector3(posX, this.pos.y + 10, posZ) );
    }

    void lookForNeighbours(Node nodeSource){

    }
}
