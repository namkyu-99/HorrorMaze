using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject Effect;
    int effectCount = 0;
    public AudioClip Complete;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GameEnd = 1;   // COMPLETE
            InvokeRepeating("SpawnEffect", 0f, 0.5f);
            AudioSource.PlayClipAtPoint(Complete, transform.position);
        }
    }
    
    void SpawnEffect()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        effectCount++;
        
        if (effectCount >= 8)
        {
            CancelInvoke("SpawnEffect");
        }
    }
}