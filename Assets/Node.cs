using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Transform transformInstance;
    private MainScript mainScript;
    public List<Node> nodeConnectionsList = new List<Node>();

    string systemName;
    float xPos;
    float yPos;

    public int health = 50;
    public int color;

    public Node(string systemName, float xPos, float yPos)
    {
        mainScript = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<MainScript>();

        color = Random.Range(0, 2);
        
        if(color == 1)
        {
            mainScript.bluePower++;
        }
        else
        {
            mainScript.redPower++;
        }

        this.transformInstance = Instantiate(mainScript.nodePrefab, new Vector2(xPos, yPos), Quaternion.identity);
        this.systemName = systemName;
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public void RemoveNode()
    {
        mainScript.RemoveNodeFromList(this);
    }
    public float GetXPos()
    {
        return this.xPos;
    }
    public float GetYPos()
    {
        return this.yPos;
    }
    public void ConnectToNode(Node node)
    {
        nodeConnectionsList.Add(node);
    }
    public void RemoveNodeConnection(Node node)
    {
        nodeConnectionsList.Remove(node);
    }
    public void ClearNodeConnections()
    {
        nodeConnectionsList.Clear();
    }

    private void SetColor()
    {
        if (color == 0)
        {
            this.transformInstance.GetComponent<SpriteRenderer>().color = new Color(1F, 0F, 0F, 1);
        }
        else if (color == 1)
        {
            this.transformInstance.GetComponent<SpriteRenderer>().color = new Color(0F, 0F, 1F, 1);
        }
    }
    private List<Node> GetFriends()
    {
        List<Node> nodes = new List<Node>();
        foreach(Node node in nodeConnectionsList)
        {
            if(node.color == this.color)
            {
                nodes.Add(node);
            }
        }

        return nodes;
    }
    private List<Node> GetEnemies()
    {
        List<Node> nodes = new List<Node>();
        foreach (Node node in nodeConnectionsList)
        {
            if (node.color != this.color)
            {
                nodes.Add(node);
            }
        }

        return nodes;
    }

    private Node GetLowestHealthNode(Node[] nodes)
    {
        Node selectedNode = nodes[0];
        foreach (Node node in nodes)
        {
            if(node.health < selectedNode.health)
            {
                selectedNode = node;
            }
        }
        return selectedNode;
    }

    private Node GetRandomNode(Node[] nodes)
    {
        return nodes[Random.Range(0,nodes.Length - 1)];
    }

    public void ReactToNeighbors()
    {
        Node[] friends = GetFriends().ToArray();
        Node[] enemies = GetEnemies().ToArray();

        int pressure = 0;

        foreach (Node enemy in enemies)
        {
            pressure -= 1;
        }

        health += pressure;

        if (health < 0)
        {
            health = 50;
            if (color == 1)
            {
                color = 0;
                mainScript.redPower++;
                mainScript.bluePower--;
            }
            else
            {
                color = 1;
                mainScript.redPower--;
                mainScript.bluePower++;
            }

        }
        if (health > 50)
        {
            health = 50;
        }
    }


    public void Tick()
    {
        ReactToNeighbors();
        SetColor();
    }
}