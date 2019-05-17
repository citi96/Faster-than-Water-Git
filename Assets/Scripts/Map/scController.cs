using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Map;
using UnityEngine;

public class scController : MonoBehaviour {
    public GameObject node;
    public GameObject nodesHolder;
	
	private bool hasStarted = false;
	private bool isFinished = false;
	
	//List of all cells created at in start
	private ArrayList cellList = new ArrayList(); 
	
	//List of cells that have been turned into rooms
	private List<scVertexNode> roomList = new List<scVertexNode>();
	
	//the Delaunay Triangulation controller 
	//(Contains incremental Algorithum for construcing a Delaunay Triangulation of a set of verticies)
	private scDTController theDTController = new scDTController();
	private bool DTFinished = false;
	
	private scPrims thePrimController = new scPrims();
	private bool PrimFinished = false;
	
	// Use this for initialization
	void Start () {
        List<int> xRange = makePositionList(60);
        List<int> yRange = makePositionList(30);

        for (int i = 0; i < 30; i++) {
            GameObject aCell = Instantiate(node);
            aCell.transform.SetParent(nodesHolder.transform, true);

            aCell.transform.localScale = new Vector3(1, 1, 1);

            int xPos = xRange[Random.Range(0, xRange.Count)];
            xRange.Remove(xPos);
            int yPos = yRange[Random.Range(0, yRange.Count)];
            yRange.Remove(yPos);
            aCell.transform.localPosition = new Vector3(xPos, yPos, 1);

            aCell.GetComponent<scCell>().setup();

            cellList.Add(aCell);
        }
		
	}

    private List<int> makePositionList(int bound) {
        List<int> range = new List<int>();
        for (int i = -bound; i <= bound; i += 1) {
            range.Add(i);
        }

        return range;
    }
	
	// Update is called once per frame
	void Update () {

        if (!hasStarted) {
            if (cellsStill()) {
                hasStarted = true;
            }
        } else {
            if (!isFinished) {

                //turn large cells into rooms;
                setRooms();

                //initalize the triangulation
                theDTController.setupTriangulation(roomList);

                isFinished = true;
            } else {
                if (!DTFinished) {
                    if (!theDTController.getDTDone()) {
                        theDTController.Update();
                    } else {
                        DTFinished = true;
                        thePrimController.setUpPrims(roomList, theDTController.getTriangulation());
                    }
                } else {

                    if (!PrimFinished) {
                        thePrimController.Update();
                        PrimFinished = true;
                    }
                }
            }
        }
	}
	
	
	//returns if all the cells have stopped moving or not
	private bool cellsStill(){
		
		bool placed = true;
		foreach (GameObject aCell in cellList){
			if (!aCell.GetComponent<scCell>().getHasStopped()){
				placed = false;
			}
		}
		return placed;
		
	}

    //handles choosing which cells to turn to rooms
    private void setRooms() {
        foreach (GameObject aCell in cellList) {
            var cellLocalPosition = aCell.transform.localPosition;
            scVertexNode thisNode = new scVertexNode(cellLocalPosition.x, cellLocalPosition.y, aCell.gameObject);
            Destroy(aCell.GetComponent<scCell>());
            roomList.Add(thisNode);
            Map.Instance.nodes.Add(thisNode.getParentCell().GetComponent<Node>());
        }
    }
}
