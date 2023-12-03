using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject text;

    GameObject[] item = new GameObject[15];
    bool isSpawned = false;

    void Start()
    {
        for(int i=0; i<15; i++)
        {
            item[i] = transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if(GameManager.instance.leftTime <= 90f && !isSpawned)
        {
            for(int i=0; i<15; i++)
            {
                item[i].SetActive(true);
            }
            isSpawned = true;
            text.SetActive(false);
        }
        else if(GameManager.instance.leftTime <= 100f && !isSpawned)
        {
            text.SetActive(true);
        }
    }
}
