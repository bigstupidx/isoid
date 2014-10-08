﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float forwardSpeed = 6f;
    public float rotationSpeed = 10f;
    public float height = 2f;

    Rigidbody playerRigidBody;
    ParticleSystem exhaustParticles;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        exhaustParticles = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(v);

        Turning(h);

        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        if (v > 0)
        {
            exhaustParticles.enableEmission = true;
        }
        else
        {
            exhaustParticles.enableEmission = false;
        }
    }

    void Move(float v)
    {
        //playerRigidBody.MovePosition(position + movement);
        playerRigidBody.AddForce(transform.forward * v * Time.deltaTime * forwardSpeed);
    }

    void Turning(float h)
    {
		playerRigidBody.AddTorque (Vector3.up * rotationSpeed * Time.deltaTime * h);
		//playerRigidBody.WakeUp ();
        //transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime * h);
    }

	void OnTriggerEnter(Collider other){
		Destroy (other.gameObject);
	}
}