using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Material Knife;
    public Material Potion;
    public Material Timer;
    public AudioClip collectSound;

    int item;   // 1: Knife, 2: Potion, 3: Timer
    float rotationSpeed = 100f;

    void OnEnable()
    {
        setItem();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Collect();
            this.gameObject.SetActive(false);
        }
    }
    
    void Collect()
    {
        switch(item)
        {
            case 1:
                GameManager.instance.knife++;
                break;
            case 2:
                GameManager.instance.potion++;
                break;
            case 3:
                GameManager.instance.timer++;
                break;
        }
    }

    public void setItem()
    {
        switch(culRandom())
        {
            case 0:             // 10%
                GetComponent<MeshRenderer>().material = Potion;
                item = 2;
                break;
            case 1:             // 20%
                GetComponent<MeshRenderer>().material = Knife;
                item = 1;
                break;
            case 2:             // 30%
                GetComponent<MeshRenderer>().material = Timer;
                item = 3;
                break;
            case 3: default:    // 40%
                this.gameObject.SetActive(false);
                break;
        }
    }

    int culRandom()
    {
        float[] probs = new float[4]{10f, 20f, 30f, 40f};
        float totalProbs = 100f;
        float randomValue = Random.value * totalProbs;
        float culValue = 0f;

        for(int i=0; i<4; i++)
        {
            culValue += probs[i];
            if(randomValue <= culValue)
                return i;
        }
        return -1;
    }
}