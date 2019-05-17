using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scDTController {
	
	private bool isDone = false;
	
	//All the triangles in the triangulation
	private List<scTriangle> triangleList = new List<scTriangle>();
	
	//Verticies that still need to be added to the triangulations
	private List<scVertexNode> toAddList = new List<scVertexNode>();
	
	//the current verticie that is being added to the triangulation
	private scVertexNode nextNode = null;
	
	//Edges that have become possibly unDelaunay due to the insertion of another verticie
	private List<scEdge> dirtyEdges = new List<scEdge>();
	
	//controls if click control should be allowed
	private bool doStep = false;
	private bool canClick;
	//controls if the algorithum should animate the process step by step
	private bool animate = false;
	//time between steps
	private float animateTime = 0.5f; 
	private float animateTimer = 0;
	//current stage the algorithum is at (used for step and animate control)
	int stage = 0;
	
	//the omega triangle created at start of triangulation
	private scTriangle rootTriangle;
	
	//the triangle the "nextNode" is inside of
	private scTriangle inTriangle;
	
	private List<scEdge> finalTriangulation = new List<scEdge>();
	
	//construvtor
	public scDTController(){
		
	}
	
	// Update is called once per frame
	public void Update () {
		
		//logic here controls the different playback modes the algorithum can be executed in
		if (doStep){
			if (Input.GetMouseButtonDown(0) && canClick){
				if (toAddList.Count>0){
					addVertexToTriangulation();
					canClick = false;
				}else{
					if (stage != 0){
						addVertexToTriangulation();
						trigDone();
					}
				}
			}
			
			if (!Input.GetMouseButtonDown(0)){
				canClick = true;	
			}
		}else{
			if (!animate){
				while(toAddList.Count>0){
					addVertexToTriangulation();
				}
				
				trigDone();
			}
		}
		
		if (animate){
			if (animateTimer < animateTime){
				animateTimer += 1 *Time.deltaTime;	
			}else{
				if (toAddList.Count > 0){
					addVertexToTriangulation();
				}else{
					if (stage != 0){
						addVertexToTriangulation();
						trigDone();
					}
				}
				animateTimer %= animateTime;
			}
		}
		
		drawTriangles();
	}
	
	public bool getDTDone(){
		return isDone;	
	}
	
	//Handles set up of triangulation
	public void setupTriangulation(List<scVertexNode> _roomList){
		
		//puts all verticies into the toDo list
		foreach(scVertexNode aNode in _roomList){
			toAddList.Add(aNode);	
		}
		
		//creates three artificial verticies for the omega triangle
		scVertexNode node0 = new scVertexNode(0,250, null);
		
		scVertexNode node1 = new scVertexNode(-250,-200, null);
		
		scVertexNode node2 = new scVertexNode(250, -200, null);
		
		//creates the omega triangle
		rootTriangle = new scTriangle(new scEdge(node0,node1), new scEdge(node0,node2), new scEdge(node1,node2));
		
		//adds the omega triangle to the triangle list
		triangleList.Add(rootTriangle);
	}
	
	//Adds a verticies to the triangulation
	private void addVertexToTriangulation(){
		//check what mode the triangulation is running in
		if (stage == 0 || (!doStep && !animate) ){
			//Find a Random verticie from the todo list
			int choice = Random.Range(0, toAddList.Count);
	
			nextNode = toAddList[choice];

			toAddList.Remove(nextNode);
			
			if (doStep || animate){
				stage++;
				return;
			}
		}
		
		if (stage == 1 || (!doStep && !animate) ){
			//stores triangles created during the loop to be appended to main list after loop
			List<scTriangle> tempTriList = new List<scTriangle>();
			
			//All edges are clean at this point. Remove any that may be left over from previous loop
			dirtyEdges.Clear();
				
			float count = -1;
			foreach(scTriangle aTri in triangleList){
				List<scEdge> triEdges = aTri.getEdges();
				count++;
				//Find which triangle the current vertex being add is located within
				if (scLineIntersector.PointInTraingle(nextNode.getVertexPosition(),triEdges[0].getNode0().getVertexPosition(),
					triEdges[0].getNode1().getVertexPosition(),triEdges[1].getNode1().getVertexPosition())){
					
					//cache the triangle we are in so we can delete it after loop
					inTriangle = aTri;
					
					//create three new triangles from each edge of the triangle vertex is in to the new vertex
					foreach(scEdge aEdge in aTri.getEdges()){
						scTriangle nTri1 = new scTriangle(new scEdge(nextNode,aEdge.getNode0()),
										new scEdge(nextNode,aEdge.getNode1()),
										new scEdge(aEdge.getNode1(),aEdge.getNode0()));
						
						//cache created triangles so we can add to list after loop
						tempTriList.Add(nTri1);	
						
						//mark the edges of the old triangle as dirty
						dirtyEdges.Add(new scEdge(aEdge.getNode0(),aEdge.getNode1()));
				
					}
					
					break;
				}
			}
			
			//add the three new triangles to the triangle list
			foreach(scTriangle aTri in tempTriList){
				triangleList.Add(aTri);	
			}
			
			//delete the old triangle that the vertex was inside of
			if (inTriangle != null){
				triangleList.Remove(inTriangle);
				inTriangle.stopDraw();
				inTriangle = null;
			}
			
			if(doStep || animate){
				stage++;
				return;
			}
		}
		
		if (stage == 2 || !doStep){
			//recursively check the dirty edges to make sure they are still delaunay
			checkEdges(dirtyEdges);
		}
				
	}
	
	private void checkEdges(List<scEdge> _list){
		
		//stores if a flip occured for mode control
		bool didFlip = false;
		
		//the current dirty edge
		if (_list.Count == 0){
			stage = 0;
			if (animate || doStep){
				if (toAddList.Count > 0){
					addVertexToTriangulation();
				}
			}
			return;
		}
		
		//get the next edge in the dirty list
		scEdge currentEdge = _list[0];
		
		scTriangle[] connectedTris = new scTriangle[2];
		int index =0;

		
		foreach(scTriangle aTri in triangleList){
			if (aTri.checkTriangleContainsEdge(currentEdge)){
				connectedTris[index] = aTri;
				index++;
			}
		}
	
		
		//in first case (omega triangle) this will = 1 so dont flip
		if (index == 2){
			//stores the two verticies from both triangles that arnt on the shared edge
			scVertexNode[] uniqueNodes = new scVertexNode[2];
			int index1= 0;
			
			//loop through the connected triangles and there edges. Checking for a vertex that isnt in the edge
			for(int i =0; i < connectedTris.Length; i++){
				foreach(scEdge aEdge in connectedTris[i].getEdges()){
					if (!currentEdge.edgeContainsVertex(aEdge.getNode0())){
						uniqueNodes[index1] = aEdge.getNode0();
						index1++;
						break;
					}
				
					if (!currentEdge.edgeContainsVertex(aEdge.getNode1())){
						uniqueNodes[index1] = aEdge.getNode1();
						index1++;
						break;
					}
				}
			}
		
		
			//find the angles of the two unique verticies
			float angle0 = calculateVertexAngle(uniqueNodes[0].getVertexPosition(), 
												currentEdge.getNode0().getVertexPosition(), 
												currentEdge.getNode1().getVertexPosition());
		
			float angle1 = calculateVertexAngle(uniqueNodes[1].getVertexPosition(), 
												currentEdge.getNode0().getVertexPosition(), 
												currentEdge.getNode1().getVertexPosition());
			
			//Check if the target Edge needs flipping
			if (angle0 + angle1 > 180){
				didFlip = true;
				
				//create the new edge after flipped
				scEdge flippedEdge = new scEdge(uniqueNodes[0], uniqueNodes[1]);
			
				//store the edges of both triangles in the Quad
				scEdge[] firstTriEdges = new scEdge[3];
				scEdge[] secondTriEdges = new scEdge[3];
			
				scVertexNode sharedNode0;
				scVertexNode sharedNode1;
				
				//set the shared nodes on the shared edge
				sharedNode0 = currentEdge.getNode0();
				sharedNode1 = currentEdge.getNode1();
				
				//construct a new triangle to update old triangle after flip
				firstTriEdges[0] = new scEdge(uniqueNodes[0], sharedNode0);
				firstTriEdges[1] = new scEdge(sharedNode0, uniqueNodes[1]);
				firstTriEdges[2] = flippedEdge;
				
				//construct a new triangle to update the other old triangle after flip
				secondTriEdges[0] = new scEdge(uniqueNodes[1], sharedNode1);
				secondTriEdges[1] = new scEdge(sharedNode1, uniqueNodes[0]);
				secondTriEdges[2] = flippedEdge;
				
				//update the edges of the triangles involved in the flip
				connectedTris[0].setEdges(firstTriEdges[0],firstTriEdges[1], firstTriEdges[2]);
				connectedTris[1].setEdges(secondTriEdges[0],secondTriEdges[1], secondTriEdges[2]);
				
				
				//Adds all edges to be potentially dirty. This is bad and should only add the edges that *could* be dirty
				foreach(scEdge eEdge in connectedTris[0].getEdges()){
					_list.Add(eEdge);	
				}
				
				foreach(scEdge eEdge in connectedTris[1].getEdges()){
					_list.Add(eEdge);	
				}
				
				//also add new edge to dirty list
				_list.Add(flippedEdge);
			}
		}

		//remove the current edge from the dirty list
		_list.Remove(currentEdge);
		
		if(doStep || animate){
			if (!didFlip){
			checkEdges(_list);
			}
		}else{
			checkEdges(_list);	
		}
	}
	
	//calculates the angle at vertex _target in triangle (_target _shared0 _shared1) in degrees
	private float calculateVertexAngle(Vector2 _target, Vector2 _shared0, Vector2 _shared1){
		float length0 = Vector2.Distance(_target, _shared0);
		float length1 = Vector2.Distance(_shared0,_shared1);
		float length2 = Vector2.Distance(_shared1, _target);
	
		return  Mathf.Acos( ((length0 * length0) + (length2 * length2) - (length1 * length1)) /(2 * length0 * length2) ) * Mathf.Rad2Deg; 
	}
	
	private void drawTriangles(){
		foreach(scTriangle aTri in triangleList){
			aTri.drawTriangle();	
		}
	}
	
	private void trigDone(){
		isDone = true;
		constructFinal();
	}
	
	//Construct a list of all the edges actually in the triangulation
	private void constructFinal(){
		foreach (scTriangle aTriangle in triangleList){
			foreach(scEdge aEdge in aTriangle.getEdges()){
				//stop edges connecting to the omega triangle to be added to the final list
				if (aEdge.getNode0().getParentCell() != null && aEdge.getNode1().getParentCell() != null){
					finalTriangulation.Add(aEdge);
				}
				
				aEdge.stopDraw();
			}
		}
	}
	
	public List<scEdge> getTriangulation(){
		return finalTriangulation;	
	}
}
