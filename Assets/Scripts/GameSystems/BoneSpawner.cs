using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSpawner : MonoBehaviour
{
    public GameObject[] bones;
    public List<Transform> spawnLocs;
    private int bonesCount = 0;
    private Transform currentSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BoneSpawn());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator BoneSpawn()
    {
        while(bonesCount < bones.Length)
        {
            yield return new WaitForSeconds(1f);
            foreach(GameObject b in bones)
            {
                int randomNumber = UnityEngine.Random.Range(0, spawnLocs.Count);
                currentSpawn = spawnLocs[randomNumber];
                b.transform.position = currentSpawn.position;
                spawnLocs.Remove(currentSpawn);
                bonesCount++;
                Debug.Log(bonesCount);
            }

        }
    }
}
