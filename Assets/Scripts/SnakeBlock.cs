using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBlock : MonoBehaviour
{
    
    public GridNode current;
    public SnakeBlock backBlock;

    private float offsetY;
    private void Start()
    {
        offsetY = transform.localScale.y * 0.5f;
    }
    public void SetTailMaterial(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }
    
    public void SetNode(GridNode Nextnode)
    {
        GridNode nd = current;
        SetPosition(Nextnode);
        if (backBlock != null) backBlock.SetNode(nd);
        
    }
    public void SetPosition(GridNode node)
    {
        if(current!=null) current.isFilled = false;
        current = node;
        current.isFilled = true;
        transform.position = ReturnPosition(node, offsetY);
    }

    Vector3 ReturnPosition(GridNode node, float offset)
    {
        Vector3 pos = node.position;
        pos.y += offset;
        return pos;
    }


}
