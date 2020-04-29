using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TraversalDirection
{
    NONE,
	LEFT,
	RIGHT,
	TOP,
	BOTTOM
}
public class GridSystemManager : MonoBehaviour
{

    public Renderer groundMaterial;


    private List<GridNode> GridNodes = new List<GridNode>();
    Vector3 initialpoisition;
    Vector3 offsetVector;


    private int rows;
    private int columns;
    private int gridCount;

    public int Rows{get{return rows;}}
    public int Columns { get { return columns; } }
    public int GridsCount { get { return gridCount; } }
    // Use this for initialization
    public void Initilize (Vector2 size)
	{
        rows    = (int)size.x;
        columns = (int)size.y;
        groundMaterial.material.mainTextureScale = size;

		//Debug.Log ("groundMaterial.bounds.size: " + groundMaterial.bounds.size);
		//Debug.Log ("groundMaterial.bounds.min: " + groundMaterial.bounds.min);
		//Debug.Log ("groundMaterial.bounds.max: " + groundMaterial.bounds.max);

		initialpoisition = groundMaterial.bounds.min;
		offsetVector = new Vector3 ((groundMaterial.bounds.size.x / rows), 0.0f, (groundMaterial.bounds.size.z / columns));
		//Debug.Log ("offsetVector" + offsetVector);
		gridCount = rows * columns;
		Vector3 position = initialpoisition + offsetVector * 0.5f;
		for (int r = 0; r<rows; r++) {
			for (int c=0; c<columns; c++) {
				Vector3 pos = position + new Vector3 (offsetVector.x * r, 0, offsetVector.z * c);
				GridNode node = new GridNode (false, pos, r, c);
				GridNodes.Add (node);
			
			}

		}

		for (int i=0; i<gridCount; i++)
		{
			GridNodes [i].left = (GridNodes [i].Column - 1 < 0) ? null : GridNodes [GetListIndex (GridNodes [i].row, GridNodes [i].Column - 1)];
			GridNodes [i].right = (GridNodes [i].Column + 1 >= columns) ? null : GridNodes [GetListIndex (GridNodes [i].row, GridNodes [i].Column + 1)];
			GridNodes [i].top = (GridNodes [i].row - 1 < 0) ? null : GridNodes [GetListIndex (GridNodes [i].row - 1, GridNodes [i].Column)];
			GridNodes [i].bottom = (GridNodes [i].row + 1 >= rows) ? null : GridNodes [GetListIndex (GridNodes [i].row + 1, GridNodes [i].Column)];

		}


	}
    public GameObject InstantiateObstacle(GameObject prefab, GridNode node, Transform container)
    {
        GameObject obj = null;
        if (!node.isFilled)
        {
            obj = Instantiate(prefab, ReturnPosition(node, prefab.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
            obj.transform.parent = container;
        }
        return obj;
    }
    public void SetRandomObstacles(GameObject prefab, Transform container, int obstacleCount)
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            GridNode n = GridNodes[Random.Range(0, gridCount)];
            if (!n.isFilled)
            {
                n.isFilled = true;
                GameObject obj = Instantiate(prefab, ReturnPosition(n, prefab.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
                obj.transform.parent = container;
            }
        }
    }
    public GameObject SetCollectObject(GameObject prefabObj, Transform container,bool instantiate,out  GridNode node )
    {
        node = null;
        GameObject obj = null;
        for (int i = 0; i < GridNodes.Count; i++)
        {
            node = GridNodes[Random.Range(0, gridCount)];
            if (!node.isFilled)
            {
                if (instantiate)
                {
                    obj = Instantiate(prefabObj, ReturnPosition(node, prefabObj.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
                    obj.transform.parent = container;
                }
                else
                {
                    prefabObj.transform.position = ReturnPosition(node, prefabObj.transform.localScale.y * 0.5f);
                }
                break;
            }
        }
       
        return obj;
    }
    public void BuildEdgeWalls(GameObject prefab, Transform container)
    {
        for (int i = 0; i < gridCount; i++)
        {
            if(GridNodes[i].left == null || GridNodes[i].right == null||
                GridNodes[i].top == null || GridNodes[i].bottom == null)
            {
                GridNodes[i].isFilled = true;
                GameObject obj = Instantiate(prefab, ReturnPosition(GridNodes[i], prefab.transform.localScale.y * 0.5f), Quaternion.identity) as GameObject;
                obj.transform.parent = container;
            }

        }
    }

    public GridNode GetRandomPosition()
    {
        for (int i = 0; i < GridNodes.Count; i++)
        {
            GridNode n = GridNodes[Random.Range(0, gridCount)];
            if (!n.isFilled) return n;
        }

        return null;
        
    }
    public GridNode GetRandomPosition(int min,int maxoffset)
    {
        for (int i = 0; i < GridNodes.Count; i++)
        {
            GridNode n = GridNodes[Random.Range(min, gridCount-maxoffset)];
            if (!n.isFilled) return n;
        }

        return null;

    }
    int GetListIndex (int r, int c)
	{
		return r * columns + c;
	}
    public int GetListIndex(GridNode node)
    {
        return node.row * columns + node.Column;
    }
    public Vector3 CheckValidPosition(Vector3 raycasethitposition, Vector2 checkGridSize, out List<GridNode> fillingNodes)
    {
        int index = GetIndexByHitPosition(raycasethitposition, checkGridSize, out fillingNodes);
        if (index < 0 && index < gridCount)
            return Vector3.zero * -1.0f;

        return GridNodes[index].position;
    }

    public int GetIndexByHitPosition(Vector3 hitposition, Vector2 size, out List<GridNode> fillingNodes)
    {
        Vector3 position = hitposition - initialpoisition;

        int r = Mathf.Clamp(Mathf.RoundToInt(position.x / offsetVector.x), 0, rows - 1);
        int c = Mathf.Clamp(Mathf.RoundToInt(position.z / offsetVector.z), 0, columns - 1);

        int startRow = r - Mathf.FloorToInt(((size.x - 1) * 0.5f));//-((size.x%2==0)?0.5f:0));
        int startColumn = c - Mathf.FloorToInt((size.y - 1) * 0.5f);//-((size.y%2==0)?0.5f:0));

        int index = GetListIndex(r, c);
        if (index >= 0 || index < gridCount)
        {
            Vector2 submatrixstart = new Vector2(startRow, startColumn);
            fillingNodes = IsValidPosition(submatrixstart, size);

            return index;
        }
        fillingNodes = null;
        return -1;

    }

    public Vector3 GetGroundHitPosition(Vector3 hitposition)
    {
        Vector3 position = hitposition - initialpoisition;

        int r = Mathf.Clamp(Mathf.RoundToInt(position.x / offsetVector.x), 0, rows - 1);
        int c = Mathf.Clamp(Mathf.RoundToInt(position.z / offsetVector.z), 0, columns - 1);

        int index = GetListIndex(r, c);

        if (index >= 0 || index < gridCount)
        {

            return GridNodes[index].position;
        }

        return Vector3.zero;

    }


    public List<GridNode> IsValidPosition(Vector2 submatrix, Vector2 size)
    {
        List<GridNode> filledNodes = null;

        if (submatrix.x < 0 || submatrix.y < 0 || submatrix.x > rows || submatrix.y > columns)
            return null;

        int rcount = Mathf.RoundToInt(submatrix.x + size.x);
        int ccount = Mathf.RoundToInt(submatrix.y + size.y);

        if (rcount > rows || ccount > columns)
            return null;

        filledNodes = new List<GridNode>();
        for (int r = (int)submatrix.x; r < rcount; r++)
        {
            for (int c = (int)submatrix.y; c < ccount; c++)
            {
                int index = GetListIndex(r, c);
                filledNodes.Add(GridNodes[index]);
                if (index >= 0 && index < gridCount)
                {
                    if (GridNodes[index].isFilled)
                        return null;
                }
                else
                    return null;
            }
        }
        return filledNodes;
    }

    public GridNode GetNextNode (GridNode node, TraversalDirection dir)
	{
        if (node == null) return null;
		GridNode n = null;
		switch (dir) {
		case TraversalDirection.TOP:
			n = node.top;
			break;
		case TraversalDirection.BOTTOM:
			n = node.bottom;
			break;
		case TraversalDirection.LEFT:
			n = node.left;
			break;
		case TraversalDirection.RIGHT:
			n = node.right;
			break;
		}
		return n;
	}
    Vector3 ReturnPosition(GridNode node,float offset)
    {
        Vector3 pos = node.position;
        pos.y += offset;
        return pos;
    }

}
