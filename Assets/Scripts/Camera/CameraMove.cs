using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public Transform player;
	public float offset;

	void Update() {
		transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z - offset);
	}
}
