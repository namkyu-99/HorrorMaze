using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public TextMeshProUGUI leftTime;

    public GameObject Minimap;

    public GameObject ItemUI;

    public Image knifeImage;
    public Image potionImage;
    public Image timerImage;
    public Image scrollImage;
    public Sprite ItemFrame_n;
    public Sprite ItemFrame_s;

    public TextMeshProUGUI knife;
    public TextMeshProUGUI _knife;
    public TextMeshProUGUI potion;
    public TextMeshProUGUI _potion;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI _timer;
    public TextMeshProUGUI description;
    public GameObject useButton;

    public GameObject GameEndUI;
    public TextMeshProUGUI GameEndText;

    public AudioSource Popup;
    public AudioSource Potion;
    public AudioSource Timer;
    public AudioSource Click;

    int min, sec;
    bool isPopup = false;
    int select;     // 0: None, 1: Knife, 2: Potion, 3: Timer, 4: Scroll
    float plusTime = 30f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateLife();
        min = (int)(GameManager.instance.leftTime / 60);
        sec = (int)(GameManager.instance.leftTime % 60);
        leftTime.text = $"{min:D2}:{sec:D2}";
        knife.text = $"{GameManager.instance.knife}";
        _knife.text = knife.text;
        potion.text = $"{GameManager.instance.potion}";
        _potion.text = potion.text;
        timer.text = $"{GameManager.instance.timer}";
        _timer.text = timer.text;

        if (Input.GetKeyDown(KeyCode.Q) && GameManager.instance.GameEnd == 0)
        {
            if(!isPopup)
            {
                isPopup = true;
                Cursor.lockState = CursorLockMode.None;
                Minimap.SetActive(true);
                ItemUI.SetActive(true);
                Popup.Play();
            }
            else
            {
                isPopup = false;
                Cursor.lockState = CursorLockMode.Locked;
                Minimap.SetActive(false);
                ItemUI.SetActive(false);
                Popup.Play();
            }
        }

        if(GameManager.instance.GameEnd != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Minimap.SetActive(false);
            ItemUI.SetActive(false);
            GameEndUI.SetActive(true);
            if (GameManager.instance.GameEnd == -1)
            {
                GameEndText.text = "GAME OVER..";
                GameEndText.color = new Color(28/255f, 24/255f, 22/255f, 1f);
            }                
            else if (GameManager.instance.GameEnd == 1)
            {
                GameEndText.text = "COMPLETE!";
                GameEndText.color = new Color(1f, 180/255f, 82/255f, 1f);
            }
        }
    }

    void UpdateLife()
    {
        switch(GameManager.instance.life)
        {
            case 0:
                life1.SetActive(false);
                life2.SetActive(false);
                life3.SetActive(false);
                break;
            case 1:
                life1.SetActive(true);
                life2.SetActive(false);
                life3.SetActive(false);
                break;
            case 2:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(false);
                break;
            case 3:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                break;
        }
    }

    public void Select(int _select)
    {
        select = _select;

        switch(select)
        {
            case 1:     // Knife
                knifeImage.sprite = ItemFrame_s;
                potionImage.sprite = ItemFrame_n;
                timerImage.sprite = ItemFrame_n;
                scrollImage.sprite = ItemFrame_n;
                description.text = "<나이프>\n마우스 우클릭을 통해 투척할 수 있다.";
                useButton.SetActive(false);
                Popup.Play();
                break;
            case 2:     // Potion
                knifeImage.sprite = ItemFrame_n;
                potionImage.sprite = ItemFrame_s;
                timerImage.sprite = ItemFrame_n;
                scrollImage.sprite = ItemFrame_n;
                description.text = "<포션>\n체력을 한 칸 회복한다.";
                useButton.SetActive(true);
                Popup.Play();
                break;
            case 3:     // Timer
                knifeImage.sprite = ItemFrame_n;
                potionImage.sprite = ItemFrame_n;
                timerImage.sprite = ItemFrame_s;
                scrollImage.sprite = ItemFrame_n;
                description.text = $"<타이머>\n남은 시간을 {plusTime}초 추가한다.";
                useButton.SetActive(true);
                Popup.Play();
                break;
            case 4:     // Scroll
                knifeImage.sprite = ItemFrame_n;
                potionImage.sprite = ItemFrame_n;
                timerImage.sprite = ItemFrame_n;
                scrollImage.sprite = ItemFrame_s;
                description.text = "<누군가의 편지>\n산책하더라도 되도록 숲에는 너무 깊이 들어가지마. 요즘 행방불명되는 사람들이 많대. 게다가 곧 기말과제 제출이라고? 집에 일찍 와서 개발이나 해!";
                useButton.SetActive(false);
                Popup.Play();
                break;
        }
    }

    public void Use()
    {
        switch(select)
        {
            case 2:     // Potion
                if(GameManager.instance.potion > 0 && GameManager.instance.life < 3)
                {
                    GameManager.instance.potion--;
                    GameManager.instance.life++;
                    Potion.Play();
                }
                else
                    Click.Play();
                break;
            case 3:     // Timer
                if(GameManager.instance.timer > 0)
                {
                    GameManager.instance.timer--;
                    GameManager.instance.leftTime += plusTime;
                    Timer.Play();
                }
                else
                    Click.Play();
                break;
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}