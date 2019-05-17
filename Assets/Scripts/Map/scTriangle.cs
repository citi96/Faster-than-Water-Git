using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scTriangle{
	
	private List<scEdge> edgeList = new List<scEdge>();
	
	private Color theDrawColor = new Color(255,0,0,1);
	
	public scTriangle(scEdge _edg0, scEdge _edg1, scEdge _edg2){
		edgeList.Add(_edg0);
		edgeList.Add(_edg1);
		edgeList.Add(_edg2);
		
		/*for (int i = 0; i < 3; i++){
			lineList[i] = new GameObject().AddComponent<LineRenderer>();
		}*/
		
		
	}
	
	public List<scEdge> getEdges(){
		return edgeList;	
	}
	
	public void drawTriangle(){
		//foreach(scEdge aEdge in edgeList){
		//	aEdge.drawEdge();
		//}
	}
	
	public void stopDraw(){
		foreach(scEdge aEdge in edgeList){
			aEdge.stopDraw();	
		}
	}
	
	
	//Find if this triangle contains a vert)
	public bool containsVertex(scVertexNode _vert){
		
		foreach(scEdge aEdge in edgeList){
			if (aEdge.getNode0() == _vert || aEdge.getNode1() == _vert){
				return true;
			}
		}
		
		return false;
	}
	
	public bool checkTriangleShareEdge(scTriangle _aTri){
		
		foreach(scEdge aEdge in _aTri.getEdges()){
			foreach (scEdge myEdge in edgeList){
				if (myEdge.checkSame(aEdge)){
					return true;	
				}
			}
			
		}
		
		return false;
		
	}
	
	public bool checkTriangleContainsEdge(scEdge _aEdge){
		foreach (scEdge myEdge in edgeList){
			if (myEdge.checkSame(_aEdge)){
				return true;	
			}
		}
		
		return false;
	}
	
	public void setEdges(scEdge _edge0, scEdge _edge1, scEdge _edge2){
		
		foreach(scEdge aEdge in edgeList){
			aEdge.stopDraw();
		}
		edgeList.Clear();
		
		edgeList.Add(_edge0);
		edgeList.Add(_edge1);
		edgeList.Add(_edge2);
	}
}
