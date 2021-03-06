﻿using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

	public float spawnTime = 3f;
	public float spawnDistance = 15f;
	public float spawnRandomDistance = 5f;
	public GameObject spawnedObject;
	
	private GameObject player;
	
	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Spawn", Random.Range(spawnTime - (spawnTime / 2), spawnTime + (spawnTime / 2)), spawnTime);
	}
	
	void Spawn()
	{
        Instantiate(spawnedObject, RandomCircle(player.transform.position, Random.Range(spawnDistance - spawnRandomDistance, spawnDistance + spawnRandomDistance)), Quaternion.Euler(-90, 0, 0));
	}
	
	Vector3 RandomCircle(Vector3 center, float radius)
	{
		int angle = Random.Range(0, 360);
		Vector3 position;
		position.x = center.x + (radius * Mathf.Cos(Mathf.Deg2Rad * angle));
		position.y = center.y;
		position.z = center.z + (radius * Mathf.Sin(Mathf.Deg2Rad * angle));
		return position;
	}
}
