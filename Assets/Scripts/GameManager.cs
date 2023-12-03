using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int GameEnd;   // 0: PLAY, -1: OVER, 1: COMPLETE

    public float life;
    public float leftTime;
    public int knife;
    public int potion;
    public int timer;
    
    public AudioSource Over;
    bool isPlayed = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameEnd = 0;
    }

    void Update()
    {
        if(GameEnd == 0)
            leftTime -= Time.deltaTime;
        if(leftTime <= 0)
        {
            leftTime = 0;
            GameEnd = -1;
            if(!isPlayed)
            {
                Over.Play();
                isPlayed = true;
            }
        }
        if(life <= 0)
        {
            GameEnd = -1;
            if(!isPlayed)
            {
                Over.Play();
                isPlayed = true;
            }
        }
    }
}