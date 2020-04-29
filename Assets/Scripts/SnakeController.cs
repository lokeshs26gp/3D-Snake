using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public GameObject bodyPrefab,tailPrefab;
    public Material bodyMat,tailMat;
    private float offsetY;
    public SnakeBlock headBlock;

    public SnakeBlock tailBlock = null;
    
    public void SetPosition(GridNode node)
    {
        headBlock.SetNode(node);
    }
    public void SetPosition(GridNode node,GridSystemManager gridSystem,TraversalDirection dir)
    {
        SetPositions(headBlock, node, gridSystem, dir);
    }
    private void SetPositions(SnakeBlock block, GridNode node,GridSystemManager gridSystem, TraversalDirection dir)
    {
        if(block!=null)
        {
           GridNode n = gridSystem.GetNextNode(node, dir);
            if(!n.isFilled)
            {
                block.SetPosition(n);
                SetPositions(block.backBlock, n, gridSystem, dir);
            }
        }
    }
    public void GenerateTail()
    {
        GameObject tail = null;
        if (tailBlock == null)
        {
           
            tail = Instantiate(tailPrefab, ReturnPosition(headBlock.current, bodyPrefab.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
            tailBlock = headBlock.backBlock = tail.GetComponent<SnakeBlock>();

        }
        else
        {
            tailBlock.SetTailMaterial(bodyMat);
            tail = Instantiate(bodyPrefab, ReturnPosition(tailBlock.current, bodyPrefab.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
            tailBlock.backBlock = tail.GetComponent<SnakeBlock>();
            tailBlock.backBlock.SetTailMaterial(tailMat);
            tailBlock = tailBlock.backBlock;
        }
        tail.transform.parent = transform;
 
    }
    Vector3 ReturnPosition(GridNode node, float offset)
    {
        Vector3 pos = node.position;
        pos.y += offset;
        return pos;
    }

}
