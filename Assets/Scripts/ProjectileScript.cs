using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	public Attack AttackData;
	public Vector3 Direction;
	public float Velocity;

	private Rigidbody rigidbody;

	private static float maxDistance = 75.0f;
	private float distanceLeft;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();

		distanceLeft = maxDistance;

		StartCoroutine ("WatchForExpiring");
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.velocity = Direction.normalized * Velocity;
		distanceLeft -= Velocity * Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!collision.gameObject.tag.Equals ("HostileProjectile")) 
		{
			Destroy (gameObject);
		}
	}

	private IEnumerator WatchForExpiring()
	{
		while (distanceLeft > 0) {
			yield return new WaitForSeconds (0.6f);
		}

		Destroy (gameObject);
	}
}
