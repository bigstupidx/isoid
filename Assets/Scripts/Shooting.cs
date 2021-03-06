﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Shooting : MonoBehaviour {

	public GameObject missile;
	public float defaultEnergyCosts = 5f;
    public float defaultFirerate = 0.2f;
	public float startingEnergy = 100f;
	public float currentEnergy = 100f;
	public float refillRate = 0.5f;
    public float nocostDuration = 5f;
    public float firerateDuration = 5f;
    public float splitDuration = 5f;
    public float splitDegrees = 20f;

    private float nextFire;
    private float energyCosts;
    private float firerate;
    private bool split = false;
    private bool cheat = false;

    public float nocostRemaining = 0f;
    public float firerateRemaining = 0f;
    public float splitRemaining = 0f;

	void OnStart() {
		currentEnergy = startingEnergy;
        energyCosts = defaultEnergyCosts;
        firerate = defaultFirerate;
	}

	// Update is called once per frame
	void Update () {
        if ((Input.GetAxis("Fire1") == 1 || CrossPlatformInputManager.GetButton("Fire1")) && Time.time > nextFire && currentEnergy - energyCosts > 0f)
        {
            nextFire = Time.time + firerate;
			Fire ();
			currentEnergy -=energyCosts;
		}
	}
	
	void FixedUpdate() {
		currentEnergy += refillRate;
		if (currentEnergy > startingEnergy) {
			currentEnergy = startingEnergy;
		}

        if (nocostRemaining > 0 && firerateRemaining > 0)
        {
            energyCosts = 0;
            firerate = defaultFirerate / 2;
            if (!cheat)
            {
                nocostRemaining -= Time.deltaTime;
                firerateRemaining -= Time.deltaTime;
            }
        }
        else if (nocostRemaining > 0)
        {
            energyCosts = 0;
            firerate = defaultFirerate;
            if (!cheat)
                nocostRemaining -= Time.deltaTime;
        }
        else if (firerateRemaining > 0)
        {
            firerate = defaultFirerate / 2;
            energyCosts = defaultEnergyCosts / 2;
            if (!cheat)
                firerateRemaining -= Time.deltaTime;
        }
        else
        {
            firerate = defaultFirerate;
            energyCosts = defaultEnergyCosts;
        }

        if (splitRemaining > 0)
        {
            split = true;
            if (!cheat)
                splitRemaining -= Time.deltaTime;
        }
        else
        {
            split = false;
        }
	}

	void Fire(){
        if (split)
        {
            Quaternion canonRotation = transform.FindChild("Canon Head").transform.rotation;
            //middle
            Instantiate(missile, transform.FindChild("Canon Head").transform.position, canonRotation);
            //left
            Instantiate(missile, transform.FindChild("Canon Head").transform.position, canonRotation * Quaternion.Euler(0, -splitDegrees, 0));
            //right
            Instantiate(missile, transform.FindChild("Canon Head").transform.position, canonRotation * Quaternion.Euler(0, splitDegrees, 0));
        }
        else
        {
            Instantiate(missile, transform.FindChild("Canon Head").transform.position, transform.FindChild("Canon Head").transform.rotation);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missile")
        {
            GetComponent<Energy>().CurrentEnergy -= other.GetComponent<MissileScript>().DestructionForce / 2;
            Destroy(other.gameObject);
        }
    }

    public void nocostPickup()
    {
        nocostRemaining = nocostDuration;
    }

    public void fireratePickup()
    {
        firerateRemaining = firerateDuration;
    }

    public void splitPickup()
    {
        splitRemaining = splitDuration;
    }


    void OnIdkfaCode()
    {
        if (!cheat)
        {
            cheat = true;
            Debug.Log("idkfa activated!");
            nocostRemaining = nocostDuration;
            firerateRemaining = firerateDuration;
            splitRemaining = splitDuration;
        }
        else
        {
            cheat = false;
            Debug.Log("idkfa deactivated!");
            nocostRemaining = 1;
            firerateRemaining = 1;
            splitRemaining = 1;
        }
    }
}
