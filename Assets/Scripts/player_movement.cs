using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {
	public int rotation_speed = 30;
	public float speed = 20f;
	public GameObject thing;
	public GameObject gem;
	public GameObject gem2;



	private Vector3 target;
	private Vector3 heading;
	private float magnitude;
	private float startSize;



	void Start () {
		startSize = Camera.main.orthographicSize;
		genSpikes (3,true);
		genGems (9, 2);
		genGems (15, 3);
		genGems (20, 4);
		genGems (30, 5);
		genGems (40, 6);
	}

	void Update () {
		magnitude = GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
		if (magnitude / 1000 < 10) {
			Camera.main.orthographicSize = startSize + (magnitude / 500);
		}
		transform.Rotate (transform.forward * rotation_speed * Time.deltaTime);
		if (Input.GetMouseButton(0)) {
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			heading =  target - transform.position;
			target.z = transform.position.z;
			transform.GetComponent<Rigidbody2D>().AddForce((heading/heading.sqrMagnitude) * speed * (heading.sqrMagnitude / 10) );
		}
	
	}   
		
	void genGems(int gems, int layer){
		int offset = 0;
		if (layer % 2 == 0) {
			offset = 0;
		}
		for (int i = 0; i < gems; i++) {
			GameObject test = Instantiate (gem2, transform.position + fromAngle (layer * 2.25f, (360 / gems * i) + offset), Quaternion.identity) as GameObject;
			test.transform.LookAt (transform.position);
			Vector3 diff = transform.position - test.transform.position;
			diff.Normalize();

			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			test.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
			test.transform.SetParent (transform);
		}
	}

	void genSpikes(int spikes, bool hasGems = false){
		GameObject spawn = thing;
		if (hasGems) {
			spikes += spikes;
		}
		for (int i = 0; i < spikes; i++) {
			if (hasGems) {
				if (i % 2 == 0) {
					spawn = gem;
				} else {
					spawn = thing;
				}
			}
			GameObject test = Instantiate (spawn, transform.position + fromAngle (2.1f, 360 / spikes * i), Quaternion.identity) as GameObject;
			test.transform.LookAt (transform.position);

			Vector3 diff = transform.position - test.transform.position;
			diff.Normalize();

			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			test.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
			test.transform.SetParent (transform);

		}
	}


	Vector3 fromAngle(float dist, float angle){
		angle = angle * Mathf.Deg2Rad;
		Vector3 tmp = Vector3.zero;
		tmp.x = Mathf.Cos (angle);
		tmp.y = Mathf.Sin (angle);
		return tmp * dist;
	}
}
