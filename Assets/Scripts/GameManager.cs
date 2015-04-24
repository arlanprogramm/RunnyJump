using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	//chr - character
	//tet - tetramino
	//pl - player
	//env - enviroment
	public int score;
	public int chrCount;
	public float tetSpeed;
	public float respTime;
	public float plSpeed;
	public float envOffsetY;
    public Color[] color;
	private ChrManager chrMng;
	private bool gameOver;

	private float incSpeedPl;
	private float startSpeed;

	void Awake() {
		chrMng = GetComponent<ChrManager>();
		startSpeed = plSpeed;
	}

	void Update() {
		if (chrMng.oneChr) {
			float maxSpeed = 2;
			incSpeedPl += Time.deltaTime * 0.1f;
			plSpeed = Mathf.Lerp (startSpeed, maxSpeed, incSpeedPl);
		}
	}

	public void GameOver() {
		Debug.Log ("");
	}
}
