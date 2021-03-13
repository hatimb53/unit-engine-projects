using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mvt: MonoBehaviour {
	public Transform Target;
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;
	public Text score;
	public Text bestScore;

	int goal=0;
	int bestGoal=0;
	private Transform myTransform;
	void Awake(){
		myTransform = transform;
	}
	Rigidbody rigidbody;
	public AudioSource crowdEnvir;
	public AudioSource ballHit;
	public AudioSource crowdGoal;
	public AudioSource kick;
	public AudioSource buzzer;

	void Start()
	{         // print ("hey1");
		
		//print ("heya");
	
		rigidbody=GetComponent<Rigidbody>();
		StartCoroutine(SimulateProjectile());
		//print ("hey2");
	}

	float abs(float x){
		if (x > 0)
			return x;
		return -x;
	}
	void OnCollisionEnter(Collision col){
		ballHit.Play ();
	}
	IEnumerator SimulateProjectile()
	{	//print ("hey3");
		// Short delay added before Projectile is thrown
		//Random rnd=new Random();
		//float rd = rnd.Next (20) * 0.1f;
		//yield return new WaitForSeconds(rd);

		// Move projectile to the position of throwing object + add some offset if needed.
		float p=0, q=0;
		Debug.Log ("dii");
		int frame1=0,frame2=0;
		while (true) {
			if (!crowdEnvir.isPlaying) {
				crowdEnvir.Play ();
			}

			// Calculate distance to target
			if (Input.GetMouseButtonDown (0)) {
				p = Input.mousePosition.x;
				q = Input.mousePosition.y;
				frame1 = Time.frameCount;

			}
			if (Input.GetMouseButtonUp (0)) {
				kick.Play ();

				frame2 = Time.frameCount;
				float a = Input.mousePosition.x;
				float b = Input.mousePosition.y;
				int df = frame2 - frame1;
				float spd = abs(a - p) / df;

					Debug.Log ("click");
				Target.position = new Vector3 ((a-p)/10, Target.position.y, Target.position.z);
				float target_Distance = Vector3.Distance (myTransform.position, Target.position);

				// Calculate the velocity needed to throw the object to the target at specified angle.
				float projectile_Velocity = target_Distance / (Mathf.Sin (2 * firingAngle * Mathf.Deg2Rad) / gravity);

				// Extract the X  Y componenent of the velocity
				float Vz = Mathf.Sqrt (projectile_Velocity) * Mathf.Cos (firingAngle * Mathf.Deg2Rad);
				float Vy = Mathf.Sqrt (projectile_Velocity) * Mathf.Sin (firingAngle * Mathf.Deg2Rad);

				// Calculate flight time.
				float flightDuration = target_Distance / Vz;

				// Rotate projectile to face the target.
				myTransform.rotation = Quaternion.LookRotation (Target.position -myTransform.position);

				float elapse_time = 0;
				float y = myTransform.position.y;
				myTransform.Translate (0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vz * Time.deltaTime);

				elapse_time += Time.deltaTime;
				print (y);
				gravity = 10f;
				while (myTransform.position.y >= y) {
					
					myTransform.Translate (0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vz * Time.deltaTime);

					elapse_time += Time.deltaTime;

					yield return null;
				}
				if (myTransform.position.z > 10 && myTransform.position.x < 4 &&myTransform.position.x > -4 &&myTransform.position.y < 4.8f) {
					goal++;
					crowdGoal.Play ();
				} 
				else {
					buzzer.Play ();
					goal = 0;

				}
				score.text = "Score : " + goal;
				if(bestGoal<goal){
					bestGoal = goal;
					bestScore.text = "Best   : " + bestGoal;

			}

				frame1 = Time.frameCount;
				frame2 = Time.frameCount;
				int tp = frame2 - frame1;

				while (tp < 150) {
					frame2 = Time.frameCount;
					tp = frame2 - frame1;


					yield return null;
				}
				myTransform.position = new Vector3 (0, 1.65f, -12);
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
			
		}
			yield return null;
	}  
}
}



