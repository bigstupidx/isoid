﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float forwardSpeed = 6f;
    public float rotationSpeed = 10f;
    public float height = 2f;

	public AudioSource pickupSound;

    Rigidbody playerRigidBody;
    ParticleSystem exhaustParticles;

    void Awake()
    {
        Time.timeScale = 1;

        playerRigidBody = GetComponent<Rigidbody>();
        exhaustParticles = GetComponentInChildren<ParticleSystem>();
    }

    void FixedUpdate()
    {
        float h = 0;
        float v = 0;

        if (Application.isMobilePlatform)
        {
			if (Input.touchCount == 1 && Input.GetTouch(0).position.y < Screen.height / 2)
			{
				if (Input.GetTouch(0).position.x < Screen.width / 3)
                {
                    h = -1;
                }
				else if(Input.GetTouch(0).position.x < Screen.width / 3 * 2)
                {
                    v = 1;
                }
				else
				{
					h = 1;
				}
            }
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        Move(v);

        Turning(h);

        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        exhaustParticles.emissionRate = 2 + v * forwardSpeed / 30;
    }

    void Move(float v)
    {
        playerRigidBody.AddForce(transform.forward * v * Time.deltaTime * forwardSpeed);
    }

    void Turning(float h)
    {
        playerRigidBody.AddTorque(Vector3.up * rotationSpeed * Time.deltaTime * h);
    }
	
    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.name == "Isoid(Clone)")
        {
			pickupSound.Play();
            float energyLeft = other.GetComponent<Energy>().CurrentEnergy;
            this.GetComponent<Energy>().CurrentEnergy += energyLeft;
			ScoreManager.score += energyLeft;
			this.GetComponent<Shooting>().currentEnergy = this.GetComponent<Shooting>().startingEnergy;
            Destroy(other.gameObject);
        }
		else if (other.gameObject.name == "PowerupNocost(Clone)")
        {
            this.GetComponent<Shooting>().nocostPickup();

            Destroy(other.gameObject);
        }
		else if (other.gameObject.name == "PowerupFirerate(Clone)")
        {
            this.GetComponent<Shooting>().fireratePickup();

            Destroy(other.gameObject);
        }
		else if (other.gameObject.name == "PowerupSplit(Clone)")
        {
            this.GetComponent<Shooting>().splitPickup();

            Destroy(other.gameObject);
        }
    }
}
