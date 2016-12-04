using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {
	public int rotation_speed = 30;
	public float speed = 20f;
	public GameObject thing;
	public GameObject gem;
	public GameObject gem2;
	float[] gemRot;



	private Vector3 target;
	private Vector3 heading;
	private float magnitude;
	private float startSize;
	[Header("Gem Settings")]
	public int gemLayers = 0;
	public float gemSkin = .5f;
	public float gemDensity = .5f;
	public float gemStartDistance = 5f;
	public Sprite gemSprite;
	

	void Start () {
		startSize = Camera.main.orthographicSize;
		genSpikes (3,true);
		//genGems (9, 2);
		//genGems (15, 3);
		//genGems (20, 4);
		//genGems (30, 5);
		//genGems (40, 6);
		gemRot = new float[2];
		genGems(gemLayers);
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
	
	void genGems(int layer)
	{
		float extents = 2 * gemSprite.bounds.extents.y;
		float theta = 0f;
		float x;
		float y;
		float delta;
		int count;

		GameObject temp;
		for (int j = 1; j <= layer; j++)
		{
			if (j == 1)
			{

				temp = Instantiate(gem) as GameObject;
				gemRot[0] = 0f;
				temp.transform.position = new Vector2(transform.position.x + gemStartDistance, transform.position.y);
				temp.transform.SetParent(transform);

				temp = Instantiate(gem) as GameObject;
				gemRot[1] = 2 * Mathf.PI / 3;
				x = Mathf.Cos(gemRot[1]);
				y = Mathf.Sin(gemRot[1]);
				temp.transform.position = new Vector2(transform.position.x + x * gemStartDistance, transform.position.y + y * gemStartDistance);
				temp.transform.SetParent(transform);


				x = Mathf.Cos(4 * Mathf.PI / 3);
				y = Mathf.Sin(4 * Mathf.PI / 3);
				temp = Instantiate(gem, new Vector2(x * gemStartDistance + transform.position.x, y * gemStartDistance + transform.position.y), Quaternion.identity) as GameObject;
				temp.transform.SetParent(transform);
			}
			else
			{
				delta = gemRot[1] - gemRot[0];
				
				float r = j * (gemSkin + extents) - extents / 2 + gemStartDistance;
				count = (int)((2 * Mathf.PI * r) / extents * .5);
				float tempTheta = delta / 2;
				theta = delta / 2;
				for (int i = 0; i < count; i++)
				{
					temp = Instantiate(gem) as GameObject;

					theta = i * 2 * Mathf.PI / count + tempTheta;

					if (i == 0)
						gemRot[0] = theta;
					else if (i == 1)
						gemRot[1] = theta;

					x = Mathf.Cos(theta);
					y = Mathf.Sin(theta);

					temp.transform.position = new Vector2(x * r + transform.position.x, y * r + transform.position.y);
					temp.transform.SetParent(transform);
				}
			}
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
