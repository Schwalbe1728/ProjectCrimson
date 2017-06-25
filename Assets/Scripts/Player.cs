using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

    private Rigidbody rigidBody;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = input * speed;
        rigidBody.velocity = velocity;
    }
}
