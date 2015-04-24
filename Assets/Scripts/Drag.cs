using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour {
	public float y;
	private float dist;
	private Transform toDrag;
	private bool dragging = false;
	private Vector3 offset;
	private Vector3 v3;

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit) && (hit.collider.tag == "Drag")) {
				toDrag = hit.transform;
				toDrag.gameObject.GetComponent<Tetramino>().isStop = true;
				if(toDrag.gameObject.GetComponent<Tetramino>().isStay())
					return;

				dist = hit.transform.position.z - Camera.main.transform.position.z;
				v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
				v3 = Camera.main.ScreenToWorldPoint(v3);
				offset = toDrag.position - v3;
				dragging = true;
			}
		}
		if (Input.GetMouseButton(0) && dragging) {
			if(toDrag.gameObject.GetComponent<Tetramino>().isStay())
				return;

			v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			v3 = Camera.main.ScreenToWorldPoint(v3);
			v3 = v3 + offset;
			v3 = new Vector3(v3.x, y, v3.z + v3.y + v3.y*0.3f);
			toDrag.position = v3;
		}
		if (Input.GetMouseButtonUp(0)) {
			if(toDrag) {
				if(toDrag.gameObject.GetComponent<Tetramino>().isStay())
					return;

				dragging = false;
				toDrag.GetComponent<Tetramino>().Stabile();
			}
		}
	}
}
