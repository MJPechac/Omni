using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gem_rotator : MonoBehaviour {
	public float rotation_speed = -20f;
	private float change_timer = 10f;
	private float cur_time = 0;
	private float set_speed = 0;
	// Use this for initialization
	void Start () {
		set_speed = Random.Range (-180, -40);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (transform.forward * rotation_speed * Time.deltaTime);
		if (Mathf.Abs(set_speed - rotation_speed) > 1) {
			rotation_speed = Mathf.Lerp (rotation_speed, set_speed, Time.deltaTime / 2);
			cur_time += Time.deltaTime;
		} else {
			set_speed = Random.Range (-180, -40);
		}
	}
}
