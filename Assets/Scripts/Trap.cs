using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioClip Damage;
    float rotationSpeed = 1000f;

    void Update()
    {
        transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if(GameManager.instance.life > 0)
            {
                GameManager.instance.life--;
                AudioSource.PlayClipAtPoint(Damage, transform.position);
            }
        }
        else if(collision.collider.CompareTag("Knife"))
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(this.gameObject);
        }
    }
}
