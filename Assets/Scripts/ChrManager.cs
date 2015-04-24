using UnityEngine;
using System.Collections;

public class ChrManager : MonoBehaviour {
    //chr - character
    public bool overChr;
    public bool oneChr;
    public GameObject chr;
    private GameManager gm;
    private ArrayList al_chr = new ArrayList();

    void Awake() {
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Start() {
        CreateChr();
        CheckChr(4815162342);
    }

    public void CheckChr(float x) {
        al_chr.Clear();

        int rightChr = 0;
        int leftChr = 0;

        var chr = GameObject.FindGameObjectsWithTag("Character");

        for (int i = 0; i < chr.Length; i++) {
            if (!chr[i].GetComponent<Character>().isDestroy) {
                al_chr.Add(chr[i]);

                float posX = chr[i].transform.position.x;

                if (posX == 0) {
                    leftChr++;
                } else if (posX == -1) {
                    rightChr++;
                }
            }
        }

        if (al_chr.Count > 1) {
            if (rightChr == 0) {
                StartCoroutine(RePosChr(al_chr, "right"));
            } else {
                if (leftChr == 0) {
                    StartCoroutine(RePosChr(al_chr, "left"));
                } else {
                    //To Forward Character
                    for (int i = 0; i < al_chr.Count; i++) {
                        GameObject _chr = (GameObject)al_chr[i];

                        if (_chr.transform.position.x == x) {
                            StartCoroutine(SmoothOffset(_chr,
                                _chr.GetComponent<Character>().offset - 1));
                        }
                    }
                }
            }
        } else {
            if (al_chr.Count == 1 && !oneChr) {
                oneChr = true;
            }

            if (al_chr.Count == 0 && !overChr) {
                gm.GameOver();
                overChr = true;
            }
        }
    }
    
    IEnumerator SmoothOffset(GameObject _chr, float offset) {
        float offsetChr = _chr.GetComponent<Character>().offset;
        float time = 0;
        
        while (time < 1f) {
            time += Time.deltaTime;
            float pos = Mathf.Lerp(offsetChr, offset, time);
            _chr.GetComponent<Character>().offset = pos;
            yield return new WaitForFixedUpdate();
        }
    }

    void CreateChr() {
        chr.GetComponent<Character>().offset = -1;

        for (int i = 0; i < gm.chrCount; i++) {
            Vector3 v3;

            if (i % 2 == 0) {
                chr.GetComponent<Character>().offset++;
                v3 = new Vector3(0, 0.3f, 0);
            } else {
                v3 = new Vector3(-1, 0.3f, 0);
            }

            Instantiate(chr, v3, Quaternion.identity);
        }

        chr.GetComponent<Character>().offset = -1;
    }

    IEnumerator RePosChr(ArrayList _chr, string side) {
        int offset = (int)_chr.Count / 2 - 1;

        for (int i = _chr.Count - 1; i >= (int)_chr.Count / 2; i--) {
            float x = 0;
            GameObject chr = (GameObject)_chr[i];

            if (side == "right")
                x = -1;
            if (side == "left")
                x = 0;

            Vector3 startPos = chr.transform.position;
            Vector3 endPos = new Vector3(x, startPos.y, startPos.z);

            float time = 0;
            while (time < 1f) {
                time += Time.deltaTime;
                chr.transform.position = Vector3.Lerp(startPos, endPos, time);
                yield return new WaitForFixedUpdate();
            }

            StartCoroutine(SmoothOffset(chr, offset));
            offset--;
        }
    }
}
