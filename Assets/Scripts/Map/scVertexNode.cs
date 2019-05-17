using UnityEngine;
using System.Collections;

public class scVertexNode {
	
	private ArrayList connectionNodes = new ArrayList();	
	private Vector2 vertexPos;
	
	GameObject parentCell;
	
	public scVertexNode(float _x, float _y, GameObject _parentCell){
		vertexPos = new Vector2(_x,_y);	
		parentCell = _parentCell;
	}
	
	public Vector2 getVertexPosition(){
		return vertexPos;
	}
	
	public void setNodes(scVertexNode _n0, scVertexNode _n1){
		connectionNodes.Add(_n0);
		connectionNodes.Add(_n1);
	}
	
	public ArrayList getConnections(){
		return connectionNodes;	
	}
	
	public GameObject getParentCell(){
		return parentCell;	
	}
}
