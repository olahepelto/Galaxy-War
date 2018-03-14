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

    public int warships;
    public int health = 10;
    public int color;

    public Node(string systemName, float xPos, float yPos)
    {
        mainScript = GameObject.FindGameObjectWithTag("ScriptObject").GetComponent<MainScript>();

        warships = Random.Range(1, 10);
        color = Random.Range(0, 2);

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

    public void Tick()
    {

        if (color == 0)
        {
            this.transformInstance.GetComponent<SpriteRenderer>().color = new Color(1F, 0F, 0F, 1);
        }
        else if(color == 1)
        {
            this.transformInstance.GetComponent<SpriteRenderer>().color = new Color(0F, 0F, 1F, 1);
        }

        if (this.warships > 0 && Random.Range(1, 24) == 2)
        {
            Node randomPathNode = nodeConnectionsList[Random.Range(0, nodeConnectionsList.Count - 1)];

            if (randomPathNode.color != this.color)
            {
                if(randomPathNode.health < 1)
                {
                    randomPathNode.color = this.color;
                    randomPathNode.health = 10;
                    randomPathNode.warships++;
                    this.warships--;
                }
                else if (randomPathNode.health > 0 && randomPathNode.warships < 1)
                {
                    this.warships--;
                    randomPathNode.health -= 2;
                }
                else if(randomPathNode.warships > this.warships)
                {
                    randomPathNode.warships -= this.warships / 2;
                    this.warships = 0;
                }
                else if (randomPathNode.warships < this.warships)
                {
                    this.warships -= randomPathNode.warships / 2;
                    randomPathNode.warships = 0;
                }
                else if (randomPathNode.warships == this.warships)
                {
                    this.warships = 0;
                    randomPathNode.warships = 0;
                }

            }
            else
            {
                randomPathNode.warships++;
            }
        }
        if(Random.Range(1, 5) == 2)
        {
            this.warships++;
        }
    }
}