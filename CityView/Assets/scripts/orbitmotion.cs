using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class orbitmotion : MonoBehaviour {
	
	// Use this for initialization


	void Start () {
		StartCoroutine (zoom ());
		StartCoroutine (transition ());
		mainSlider=GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider> ();
		mainSlider.onValueChanged.AddListener (startCo);



	}



	float min=30f,max=90f,sensitivity=10f;
	float calculateDistance(Vector2 p1,Vector2 p2){

		return Mathf.Sqrt((p1.x-p2.x)*(p1.x-p2.x)+(p1.y-p2.y)*(p1.y-p2.y));
	}

	IEnumerator zoom(){
		Debug.Log ("trans");
		while (true) {
			Debug.Log ("loop");
			if(Input.touchCount==2){
				//Camera.main.fieldOfView++;
				Debug.Log ("touch");
				Touch t1 = Input.GetTouch (0);
				Touch t2 = Input.GetTouch (1);
				Vector2 tp1 = t1.position - t1.deltaPosition;
				Vector2 tp2 = t2.position - t2.deltaPosition;
				if (calculateDistance (t1.position, t2.position) > calculateDistance (tp1, tp2)) {
					if (Camera.main.fieldOfView >min) {
						Camera.main.fieldOfView--;
					}
				}
				else if (calculateDistance (t1.position, t2.position) < calculateDistance (tp1, tp2)) {
					if (Camera.main.fieldOfView <max) {
						Camera.main.fieldOfView++;
					}
				}
			}

			yield return null;
		}
	}

	Camera cam;
	public float orbitAmount=5;
	Vector2 mp,mn;
	// Update is called once per frame
	int count=0;
	IEnumerator transition(){
		while (true) {

			transform.LookAt (targ);
			Debug.Log ("transition");
			if (Input.touchCount==1&&Input.GetMouseButton (0)) {
				Debug.Log ("click");
				mp = Input.mousePosition;
				while (count < 1) {
					count++;
					Debug.Log (count);
					yield return null;
				}
				mn = Input.mousePosition;
				count = 0;
				if (mn.x > mp.x) {

					Debug.Log ("+");
					transform.RotateAround (targ.position, Vector3.up, orbitAmount);
				} else if (mn.x < mp.x) {
					Debug.Log ("-");
					transform.RotateAround (targ.position, Vector3.down, orbitAmount);
				}

			}

			yield return null;
		}
	}
	public Transform target=null,targ=null;
	public Slider mainSlider;

	float sp=1;
	// Update is called once per frame

	void dfcityView(){
		target = GameObject.FindGameObjectWithTag("DefaultCityv").GetComponent<Transform> ();
		targ=GameObject.FindGameObjectWithTag ("DefaultCityt").GetComponent<Transform> ();
	}
	
	void lakeView(){
		target=GameObject.FindGameObjectWithTag ("Lakev").GetComponent<Transform> ();
		targ=GameObject.FindGameObjectWithTag ("Laket").GetComponent<Transform> ();
	}
	void signBoardView(){
		target=GameObject.FindGameObjectWithTag ("SignBoardv").GetComponent<Transform> ();
		targ=GameObject.FindGameObjectWithTag ("SignBoardt").GetComponent<Transform> ();
	}
	void towerView(){
		target=GameObject.FindGameObjectWithTag ("Towerv").GetComponent<Transform> ();
		targ=GameObject.FindGameObjectWithTag ("Towert").GetComponent<Transform> ();
	}
	void startCo(float c){
		
		StopAllCoroutines ();


		StartCoroutine (movetodefault ());
	}
	IEnumerator movetotarget(){
		
		Debug.Log ("target");
		int p = 1;
		switch ((int)mainSlider.value) {
		case 0:
			dfcityView ();
			break;
		case 1:
			lakeView ();
			break;
		case 2:
			towerView ();
			break;
		case 3:
			signBoardView ();
			break;
		}
		int count = 0;
		Vector3 prevDir=new Vector3(0,0,0);
		while (true) {
			Vector3 targetDir = targ.position - transform.position;

			float step = 0.5f * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			if (prevDir != null) {
				Debug.Log (newDir.x + " " + newDir.y + " " + newDir.z + " " + prevDir.x + " " + prevDir.y + " " + prevDir.z+" "+count++);
				if ((int)(newDir.x*10000) ==(int)( prevDir.x*10000) &&(int) (newDir.y*10000) == (int)(prevDir.y*10000) && (int)(newDir.z*10000) == (int)(prevDir.z*10000)) {
					break;
				}
			}
			Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);
			prevDir = newDir;
			//Debug.Log (count++);
			yield return null;

		}
			while (true) {
			
			float sp = 1;
			float vx = (int)(transform.position.x - target.position.x);
			float vy =(int) (transform.position.y - target.position.y);
			float vz = (int)(transform.position.z -target.position.z);

			if (vx != 0) {
				if (vx > 0) {
					sp = -1;
				} else {
					sp = 1;
				}
				Debug.Log ("vx");
				transform.position = new Vector3 (transform.position.x + sp, transform.position.y, transform.position.z);
				Debug.Log ("vx1");
			}
			if (vx == 0 && vz != 0) {
				if (vz > 0) {
					sp = -1;
				} else {
					sp = 1;
				}
				Debug.Log ("vz");
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + sp);
			}
			if (vx == 0 && vz == 0 && vy != 0) {
				if (vy > 0) {
					sp = -1;
				} else {
					sp = 1;
				}
				Debug.Log ("vy");
				transform.position = new Vector3 (transform.position.x, transform.position.y + sp, transform.position.z);
			}
			if (vy == 0) {
				Debug.Log ("vo");
				mainSlider.enabled = true;
				break;
			}
			Debug.Log ("2");
			transform.LookAt (targ);
			Debug.Log ("3");
			yield return null;
		}
		StartCoroutine (zoom ());
		StartCoroutine (transition());

	}

	IEnumerator movetodefault(){
		while (Camera.main.fieldOfView != 51) {
			if (Camera.main.fieldOfView > 51) {
				Camera.main.fieldOfView--;
			} else if (Camera.main.fieldOfView < 51) {
				Camera.main.fieldOfView++;
			}
			yield return null;

		}
		mainSlider.enabled = false;
		Debug.Log ("default");
		dfcityView ();
		int count = 0;
		Vector3 prevDir=new Vector3(0,0,0);
		while (true) {
			Vector3 targetDir = targ.position - transform.position;

			float step = 0.5f * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			if (prevDir != null) {
				Debug.Log (newDir.x + " " + newDir.y + " " + newDir.z + " " + prevDir.x + " " + prevDir.y + " " + prevDir.z+" "+count++);
				if ((int)(newDir.x*10000) ==(int)( prevDir.x*10000) &&(int) (newDir.y*10000) == (int)(prevDir.y*10000) && (int)(newDir.z*10000) == (int)(prevDir.z*10000)) {
					break;
				}

			}
			Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);
			prevDir = newDir;
			//Debug.Log (count++);
			yield return null;
		}

		while(true){
			float sp = 1;
			float vx = (int)(transform.position.x - target.position.x);
			float vy = (int)(transform.position.y - target.position.y);
			float vz = (int)(transform.position.z - target.position.z);

			if (vy != 0) {
				if (vy > 0) {
					sp = -1;
				} else {
					sp =1;
				}
				Debug.Log ("vx");
				transform.position = new Vector3 (transform.position.x, transform.position.y + sp, transform.position.z);
			}
			if (vy == 0 && vz != 0) {
				if (vz > 0) {
					sp = -1;
				} else {
					sp = 1;
				}
				Debug.Log ("vz");
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + sp);
				Debug.Log ("0");
			}
			if (vy == 0 && vz == 0 && vx != 0) {
				Debug.Log ("v1");
				if (vx > 0) {
					Debug.Log ("v2");
					sp = -1;
				} else {
					Debug.Log ("v3");
					sp = 1;
				}
				Debug.Log ("vy");
				transform.position = new Vector3 (transform.position.x + sp, transform.position.y, transform.position.z);
			}
			if(vx==0){
				Debug.Log ("br");
				break;
			}
			Debug.Log ("v1");
			transform.LookAt (targ);
			Debug.Log ("v1");
			yield return null;
			Debug.Log ("v1");
		}
		Debug.Log ("");
		StartCoroutine(movetotarget ());

	}	


}
