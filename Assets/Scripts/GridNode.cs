using UnityEngine;
using System.Collections;

[System.Serializable]
public class GridNode 
{

	public bool isFilled;
	public Vector3 position;
	public int row,Column;
	public GridNode left,right,top,bottom;

	public GridNode(bool pfilled,Vector3 pposition,int prow,int pcolumn)
	{
		this.isFilled = pfilled;
		this.position = pposition;
		this.row      = prow;
		this.Column   = pcolumn;
	
	}

}
