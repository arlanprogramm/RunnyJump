using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private int posZ;
	private GameManager gm;

	void Start() {
		gm = GameObject.Find("Manager").GetComponent<GameManager>();
		posZ = (int) transform.position.z;
	}

	void Update () {
		if ((int) transform.position.z != posZ 
		    	&& transform.position.z < 0 && gm.chrCount > 0) {
			gm.score++;
			posZ = (int) transform.position.z;
		}

		if (gm.chrCount > 0)
			transform.Translate(Vector3.back * Time.deltaTime * gm.plSpeed);
	}
}
