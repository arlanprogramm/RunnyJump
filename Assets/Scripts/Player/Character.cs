using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public float offset;
	private Transform head;
	private Transform player;
	private Transform groundForward;
	private Transform groundBack;
    private Transform[] headSide = new Transform[4];
    private bool checkGroundForward;
    private bool checkGroundBack;
    private bool isFall;
	public bool isDestroy;
	private GameManager gm;
	private ChrManager chrMng;

	void Start() {
		chrMng = GameObject.Find("Manager").GetComponent<ChrManager>();
		gm = GameObject.Find("Manager").GetComponent<GameManager>();
		groundForward = transform.FindChild("CheckGroundForward");
		groundBack = transform.FindChild("CheckGroundBack");
		player = GameObject.Find("Player").transform;
		head = transform.FindChild("Head");

		for (int i = 1; i <= headSide.Length; i++) 
			headSide[i - 1] = transform.FindChild("CheckHead" + i);
	}

	void Update() {
		Vector3 pos = transform.position;

		if (!isFall) {
            //Move Character
			transform.position = new Vector3 (pos.x, 0.3f, player.position.z + offset);

            //Ground Detected
			checkGroundForward = Physics.Linecast(pos, groundForward.position, 
                        						1 << LayerMask.NameToLayer ("Tetramino"));
			checkGroundBack    = Physics.Linecast(pos, groundBack.position, 
	                                      		1 << LayerMask.NameToLayer ("Tetramino"));
		} else if(!isDestroy) {
			int sideCount = 0;

            //Round position
			transform.position = new Vector3(Mathf.Round (pos.x), 
                                             -0.66f, 
                                             Mathf.Round (pos.z));
            
            //Increase head's scale
			head.transform.localScale = new Vector3(1,1,1);

			gm.chrCount--;
			isDestroy = true;

			chrMng.CheckChr(pos.x);

			for(int i = 0; i < headSide.Length; i++) {
				bool check = Physics.Linecast(transform.position, headSide[i].position, 
				                              1 << LayerMask.NameToLayer ("Tetramino"));
				if(check) {
					sideCount++;
				}
			}
			
            //Character falls with Rigidbody
			if(sideCount < 3) {
				gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
			}

            //Character's set layer Tetramino
			gameObject.layer = 8;
		}

        //Character Falls
		if (!checkGroundForward && !checkGroundBack)
			isFall = true;
	}
}
