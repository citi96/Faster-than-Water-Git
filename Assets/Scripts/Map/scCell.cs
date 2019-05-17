using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scCell : MonoBehaviour {
	
	private float squareDistance;
	
	float xShift = 0;
	float yShift = 0;
	
	private bool hasStopped;
	private Vector2 oldPos = new Vector2();
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float strength;
		
		GameObject[] allCells = GameObject.FindGameObjectsWithTag("Node");
		
		for (int i = 0; i < allCells.Length; i++){
			
			if (allCells[i] != this.gameObject){
				if (isOverlapping(allCells[i])){
					Vector3 direction = transform.localPosition - allCells[i].transform.localPosition;
					
					strength = 1;
					
					direction.Normalize();
					
					xShift += strength * direction.x;
					yShift += strength * direction.y;
				}
			}
		}
		
		transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x + xShift), Mathf.Round(transform.localPosition.y + yShift),transform.localPosition.z);
		xShift = 0;
		yShift = 0;
		
		
		if (transform.localPosition.x == oldPos.x && transform.localPosition.y == oldPos.y){
			hasStopped = true;
		}else{
			hasStopped = false;	
		}
		
		
		oldPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
	}
	
	public void setup(){
		transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), transform.localPosition.z);
		
		foreach(Transform aChild in transform){
			aChild.gameObject.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
		}
	}
		
	private bool isOverlapping(GameObject aObj){
		
		Vector2 mySize =  new Vector2(GetComponent<Image>().sprite.bounds.size.x,GetComponent<Image>().sprite.bounds.size.y);
		
		if (pointInside(aObj, new Vector2(transform.localPosition.x+1 - mySize.x/2, transform.localPosition.y+1 - mySize.y/2) )){
			return true;	
		}
		
		if (pointInside(aObj, new Vector2(transform.localPosition.x-1 + mySize.x/2, transform.localPosition.y+1 - mySize.y/2 ))){
			return true;	
		}
		
		if (pointInside(aObj, new Vector2(transform.localPosition.x+1 - mySize.x/2,transform.localPosition.y-1 + mySize.y/2) )){
			return true;	
		}
		
		if (pointInside(aObj, new Vector2(transform.localPosition.x-1 + mySize.x/2,transform.localPosition.y-1 + mySize.y/2) )){
			return true;	
		}
		
		return false;
	}


    private bool pointInside(GameObject aObj, Vector2 _point) {
        Vector2 objSize = new Vector2(aObj.GetComponent<Image>().sprite.bounds.size.x, aObj.GetComponent<Image>().sprite.bounds.size.y);
        if ((_point.x) >= (aObj.transform.localPosition.x - objSize.x / 2) &&
             (_point.x) <= (aObj.transform.localPosition.x + objSize.x / 2) &&
             (_point.y) >= (aObj.transform.localPosition.y - objSize.y / 2) &&
             (_point.y) <= (aObj.transform.localPosition.y + objSize.y / 2)) {
            return true;
        }

        return false;
    }
	
	public bool getHasStopped(){
		return hasStopped;	
	}
}
