using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {


    private List<Node> nodeList = new List<Node>();
    public Transform nodePrefab;

    public LineFactory lineFactory;
    private Line drawnLine;

    public int bluePower = 0;
    public int redPower = 0;

    public bool RUNNING = true;

    void Start()
    {
        lineFactory = GameObject.FindGameObjectWithTag("LineFactory").GetComponent<LineFactory>();
        nodePrefab = Resources.Load<Transform>("Node");
        GenerateNodes();
        GenerateNodeConnections();
    }

    public void RemoveNodeFromList(Node node)
    {
        Destroy(node.transformInstance);
        nodeList.Remove(node);
        node = null;
    }

    public void GenerateNodes()
    {
        int WIDTH = 40;
        int HEIGHT = 19;

        float XOFFSET = 0F;
        float YOFFSET = 0F;

        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                float x = Random.Range(0F, 0.75F);
                float y = Random.Range(0F, 0.75F);
                float distance = GetDistanceToNearestNode(x, y);

                nodeList.Add(new Node("Delta X-166 Epsilon", i + x - 19.5F + XOFFSET, j + y - 9F + YOFFSET));
            }
        }
    }


    private void GenerateNodeConnections()
    {
        
        foreach (Node node1 in this.nodeList)
        {
            for (int i = 0; i < 30; i++)
            {
                Node node2 = this.nodeList[Random.Range(0, this.nodeList.Count - 1)];

                float distance = GetDistanceBetweenNodes(node1, node2);

                if (distance < 1.5 && node1.GetXPos() != node2.GetXPos() && node1.GetYPos() != node2.GetYPos())
                {
                    ConnectNodeToNode(node1, node2);
                }
            }
        }
        

        foreach (Node node1 in this.nodeList)
        {
            if (node1.nodeConnectionsList.Count < 2)
            {
                Node[] closeNodes = GetClosestNode(node1);

                ConnectNodeToNode(node1, closeNodes[0]);
                ConnectNodeToNode(node1, closeNodes[1]);
            }
        }
    }

    private void ConnectNodeToNode(Node node1, Node node2)
    {
        drawnLine = lineFactory.GetLine(node1.transformInstance.position, node2.transformInstance.position, 0.01f, Color.gray);
        node1.ConnectToNode(node2);
        node2.ConnectToNode(node1);
    }

    public void ClearLines()
    {
        var activeLines = lineFactory.GetActive();

        foreach (var line in activeLines)
        {
            line.gameObject.SetActive(false);
        }
    }

    Node[] GetClosestNode(Node node1)
    {
        float smallestDistance = 100000.0F;

        Node[] closeNodes = new Node[2] {node1,node1};

        foreach (Node node2 in this.nodeList)
        {
            float nodeDistance = GetDistanceBetweenNodes(node1, node2);

            if (nodeDistance < smallestDistance && nodeDistance != 0)
            {
                smallestDistance = nodeDistance;
                closeNodes[1] = closeNodes[0];
                closeNodes[0] = node2;
                
            }
        }

        return closeNodes;
    }

    float GetDistanceToNearestNode(float x, float y)
    {
        float smallestDistance = 100000.0F;

        foreach (Node node in this.nodeList)
        {
            float x_distance = x - node.GetXPos();
            float y_distance = y - node.GetYPos();
            float nodeDistance = Mathf.Pow(x_distance, 2F) + Mathf.Pow(y_distance, 2F);
            nodeDistance = Mathf.Sqrt(nodeDistance);

            if (nodeDistance < smallestDistance && nodeDistance != 0)
            {
                smallestDistance = nodeDistance;
            }
        }

        return smallestDistance;
    }

    float GetDistanceBetweenNodes(Node node1, Node node2)
    {
        float x_distance = node1.GetXPos() - node2.GetXPos();
        float y_distance = node1.GetYPos() - node2.GetYPos();

        float distance = Mathf.Pow(x_distance, 2F) + Mathf.Pow(y_distance, 2F);
        distance = Mathf.Sqrt(distance);

        return distance;
    }

	void Update () {

        if (!RUNNING)
        {
            return;
        }

        foreach (Node node in nodeList)
        {
            for (int i = 0; i < 1; i++)
            {
                node.Tick();
            }
            
        }
        if (bluePower < 10 || redPower < 10)
        {
            RUNNING = false;
            Debug.Log("SIMULATION ENDED!");
        }
    }
}
