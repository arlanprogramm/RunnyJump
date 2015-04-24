using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {
	public float startTime;
	public float offset;
	public Transform cam;
	public GameObject[] objs;
	private int[] angle = new int[3] {90, 180, 270};
	private GameManager gm;

	void Awake() {
		gm = GameObject.Find("Manager").GetComponent<GameManager>();
	}

	void Start() {
		InvokeRepeating("Inst", startTime, gm.respTime);
	}

	void Update() {
		transform.position = new Vector3(transform.position.x, 
		                                 transform.position.y, 
		                                 cam.position.z - offset);
	}

	void Inst() {
		Instantiate(objs[Random.Range(0, objs.Length)], 
		            transform.position,
		            Quaternion.Euler(new Vector3(0, 
                 					angle[Random.Range(0, angle.Length)], 
		                         										0)));
	}
}
