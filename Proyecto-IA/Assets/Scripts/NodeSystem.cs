using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSystem : Singleton<NodeSystem>
{

    [SerializeField]GameObject _terrain;
    bool isInit = false;

    //MapLenght
    Vector3 pos;
    float height = 0;
    float width = 0;
    Vector3 center;
    //[SerializeField] LayerMask layerMask;

    //NodeInfo
    List<Node> _nodes;
    int nodeCant;
    [SerializeField] float density = 0.1f;
    int verticalNodes = 0;
    int horizontalNodes = 0;

    //raycast
    RaycastHit hit;
    //Node Pathfinding
    List<Node> openNodes;

    void Start()
    {
        _nodes = new List<Node>();
        if (_terrain){
            init();
        }
        //StartCoroutine("updateObstacles");
    }

    private void Update()
    {

    }

    void init(){
        isInit = true;
        pos = _terrain.transform.position;
        BoxCollider boxC = _terrain.GetComponent<BoxCollider>();
        center = boxC.bounds.center;
        width = boxC.bounds.max.z - boxC.bounds.min.z;
        height = boxC.bounds.max.x - boxC.bounds.min.x;
        Debug.Log("height: " + height + " Width:" + width);

        verticalNodes = Mathf.RoundToInt(height * density / 2);
        horizontalNodes = Mathf.RoundToInt(width * density / 2);
        Debug.Log("Verical: " + verticalNodes);
        Debug.Log("Horizontal:" + horizontalNodes);
        //Node creation
        createNode(pos.x, pos.z);
        float XSpacing = 1 / density;
        float YSpacing = 1 / density;
        //topRight
        fillNodes(XSpacing , YSpacing);
        //bottomRight
        //fillNodes(XSpacing, -YSpacing);
        //topLeft
        //fillNodes(-XSpacing, YSpacing);
        //bottomLeft
        //fillNodes(-XSpacing, -YSpacing);
    }


    void createNode(float posX, float posZ){
        //Pos X Z pasada, y la pos del terreno + 1 en Y
        Node node = new Node(new Vector3(posX, this.pos.y + 1f, posZ) );
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(posX, pos.y + 10, posZ), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            Debug.Log("Did Hit");
            if (hit.collider.gameObject.layer == 9){
                node.obstacle = false;
            }
            Debug.Log("Hit: " + hit.collider.name);
        }
        _nodes.Add(node);
        nodeCant++;
    }

    void fillNodes(float offsetX, float offsetY){
        for (int i = 0; i < horizontalNodes; i++)
        {
            for (int j = 0; j < verticalNodes; j++)
            {
                if(j == 0 && i == 0){
                    break;
                }
                createNode(offsetX * i, offsetY * j);
            }
        }
    }

    void lookForNeighbours(Node nodeSource){

    }

    IEnumerator updateObstacles(){
        Debug.Log("test");
        foreach(Node node in _nodes){
            if (Physics.Raycast(new Vector3(node.pos.x, pos.y + 10, node.pos.z), transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                Debug.Log("Did Hit");
                if (hit.collider.gameObject.layer == 9) {
                    node.obstacle = false;
                } else {
                    node.obstacle = true;
                }
                Debug.Log("Hit: " + hit.collider.name);
            }
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine("updateObstacles");
    }

    private void OnDrawGizmos() {
        if (Application.isPlaying) {
            foreach (Node nodo in _nodes) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(nodo.pos, 0.5f);
                Debug.Log("MOSTRANDO");
            }
        }
    }
}
