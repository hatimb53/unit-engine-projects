using UnityEngine;
using System.Collections;

public class loopanimated : MonoBehaviour {

	// Use this for initialization
	void Start () {
		p = sp;
	}

	// Update is called once per frame
	float sp=0.15f;
	public float x=0;
	float p = 0;

	void Update () {


		if (x >= 3.75f) {
			p =- sp;
		} else if (x <= -3.75f) {
			p = sp;
		}
		x = x + p;
		transform.position = new Vector3(x, 2.75f,8.75f);

		//print(x);
	}
}
