using Map;
using UnityEngine;
public class scEdge {

	private scVertexNode node0;
	private scVertexNode node1;
	
	private LineRenderer theLine;

    private GameObject dot;
    private GameObject holder;


    public scEdge(scVertexNode _n0, scVertexNode _n1){
        if (holder == null) {
            
        }

        node0 = _n0;
		node1 = _n1;

        //theLine = new GameObject().AddComponent<LineRenderer>();
        //theLine.transform.parent = holder.transform;
        //theLine.name = "EdgeLine";
        //theLine.tag = "Lines";
    }
	
	public scVertexNode getNode0(){
		return node0;
	}
	
	public scVertexNode getNode1(){
		return node1;	
	}
	
	public bool checkSame(scEdge _aEdge){
		if 	( (node0 == _aEdge.getNode0() || node0 == _aEdge.getNode1()) &&
			  (node1 == _aEdge.getNode0() || node1 == _aEdge.getNode1())){
			return true;
		}
		
		return false;
	}
	
	public bool edgeContainsVertex(scVertexNode _aNode){
		if (node0 == _aNode || node1 == _aNode){
			return true;	
		}
		
		return false;
	}
	
	public void drawEdge(){
		if(node0.getParentCell() != null && node1.getParentCell() != null){
            //if (theLine == null) {
            //    theLine = new GameObject().AddComponent<LineRenderer>();
            //    theLine.name = "EdgeLine";
            //    theLine.tag = "Lines";
            //}

            //dot = Resources.Load("Prefabs/Dot") as GameObject;
            //instantiateDots();

            //theLine.SetWidth(0.2f, 0.2f);
            //theLine.material = new Material(Shader.Find("Unlit/Texture")) {
            //    color = Color.black
            //};
            //theLine.SetVertexCount(2);
            //theLine.SetPosition(0, new Vector3(node0.getVertexPosition().x, node0.getVertexPosition().y, 1));
            //theLine.SetPosition(1, new Vector3(node1.getVertexPosition().x, node1.getVertexPosition().y, 1));
            //theLine.transform.parent = holder.transform;
        }
	}

    public void stopDraw(){
		if (theLine != null){
			GameObject.Destroy(theLine.gameObject);	
		}
	}

    public void setupNode() {
        if (!node0.getParentCell().GetComponent<Node>().Neighbors.Contains(node1.getParentCell().GetComponent<Node>())) {
            node0.getParentCell().GetComponent<Node>().AddNeighbor(node1.getParentCell().GetComponent<Node>());
        }
        if (!node1.getParentCell().GetComponent<Node>().Neighbors.Contains(node0.getParentCell().GetComponent<Node>())) {
            node1.getParentCell().GetComponent<Node>().AddNeighbor(node0.getParentCell().GetComponent<Node>());
        }
    }
}
