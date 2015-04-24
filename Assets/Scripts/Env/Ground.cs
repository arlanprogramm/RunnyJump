using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
    public float offsetZ;
    public float offsetY;
    public GameObject[] ground;

    private int groundCount;
    private float grScaleZ;
    private float posZ;
    private float tempPosZ;
    private Transform pl;
    private GameManager gm;

    void Awake() {
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        pl = GameObject.Find("Player").transform;
    }

    void Start() {
        grScaleZ = ground[0].transform.localScale.z;
        posZ = pl.position.z;
        tempPosZ = (posZ + offsetZ);
        do {
            int tGround = (groundCount % 2 == 0) ? 1 : 0;
            Instantiate(ground[tGround], new Vector3(0, offsetY, tempPosZ), Quaternion.identity);
            tempPosZ -= grScaleZ;
            groundCount++;
        } while (tempPosZ >= -grScaleZ * 2);
    }

    void Update() {
        var grounds = GameObject.FindGameObjectsWithTag("Ground");
        float realPlPosZ = pl.position.z;

        if ((int)realPlPosZ == (int)(posZ - Mathf.Round(grScaleZ))) {
            Destroy(grounds[0]);
            int tGround = (groundCount % 2 == 0) ? 1 : 0;
            Instantiate(ground[tGround], new Vector3(0, offsetY, tempPosZ), Quaternion.identity);
            tempPosZ -= grScaleZ;
            groundCount++;
            posZ = realPlPosZ;
        }
    }
}
