using UnityEngine;
using System.Collections;

public class Tetramino : MonoBehaviour {
    public Transform[] _out;
    public bool[] b_out;
    public Material[] material;
    public GameObject[] childCube;
    public bool isCollision;
    public bool isStop;
    private bool stay;
    private GameManager gm;

    void Awake() {
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Start() {
        //Set color on start
        StartCoroutine(ChangeColor(0));
    }

    void Update() {
        //Side detected
        for (int i = 0; i < _out.Length; i++) {
            b_out[i] = Physics.Linecast(transform.position, _out[i].position,
                                        1 << LayerMask.NameToLayer("Tetramino"));
        }

        //Set blue color because tetramino collided different tetramino
        for (int i = 0; i < b_out.Length; i++) {
            if (b_out[i] && !isCollision) {
                StartCoroutine(ChangeColor(1));
                break;
            }
        }

        int colCount = new int();
        for (int i = 0; i < b_out.Length; i++) {
            if (!b_out[i])
                colCount++;
        }
        if (colCount == b_out.Length)
            StartCoroutine(ChangeColor(0));

        //Move tetramino
        if (!isStop) {
            transform.Translate(Vector3.forward * Time.deltaTime * gm.tetSpeed, Space.World);
        }
    }

    public void Stabile() {
        for (int i = 0; i < _out.Length; i++)
            if (b_out[i] && !isCollision) {
                RoundPlace();
                break;
            }
    }

    private void RoundPlace() {
        var pos = transform.position;
        transform.position = new Vector3(Mathf.Round(pos.x),
                                         Mathf.Round(pos.y),
                                         Mathf.Round(pos.z));
        StartCoroutine(ChangeColor(1));
        Destroy(GetComponent<Rigidbody>());
        gameObject.layer = 8;
        stay = true;

        BoxCollider[] cols = GetComponents<BoxCollider>();
        foreach (BoxCollider col in cols) {
            if (col.isTrigger) {
                //Destroy trigger for touch
                Destroy(col);
            } else {
                col.size = new Vector3((int)col.size.x + 1,
                                       0.5f,
                                       (int)col.size.z + 1);
            }
        }
    }

    public bool isStay() {
        if (stay)
            return true;
        else
            return false;
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Drag" && !isStop) {
            RoundPlace();
            isStop = true;
        }
    }

    void OnCollisionStay(Collision other) {
        //Этот объект врезался в другой объект
        isCollision = true;
        if (!stay) {
            StartCoroutine(ChangeColor(2));
        }
    }

    void OnCollisionExit(Collision other) {
        //Этот выходит из другого объекта
        if (!stay) {
            isCollision = false;
            StartCoroutine(ChangeColor(0));
        }
    }

    IEnumerator ChangeColor(int color) {
        float time = 0;
        Color startColor = childCube[0].GetComponent<Renderer>().material.color;

        while (time < 1f) {
            time += Time.deltaTime * 3;

            for (int i = 0; i < childCube.Length; i++) {
                childCube[i].GetComponent<Renderer>().material.color = Color.Lerp(startColor, gm.color[color], time);
            }

            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < childCube.Length; i++) {
            childCube[i].GetComponent<Renderer>().material.color = gm.color[color];
        }
    }
}
