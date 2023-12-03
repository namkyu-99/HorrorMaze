using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    bool isCollision = false;
    bool isFirstCollision = true;
    float RotateSpeed = 1000f;
    float spawnTime;
    AudioSource audioSource;
    public AudioClip KnifeHit;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!isCollision)
            transform.Rotate(Vector3.right * Time.deltaTime * RotateSpeed);
        
        spawnTime += Time.deltaTime;
        if(spawnTime > 5f)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().useGravity = true;
        isCollision = true;
        if(isFirstCollision)
        {
            audioSource.clip = KnifeHit;
            audioSource.Play();
            isFirstCollision = false;
        }
    }
}