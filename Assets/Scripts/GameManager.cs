using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> coinsList ;
    public List<GameObject> powShroomsList ;
    public List<GameObject> oneUpShroomsList ;
    public List<GameObject> starsList ;
    public GameObject coin;
    public GameObject powShroom;
    public GameObject oneUpShroom;
    public GameObject star;
    public float undergroundPositionX;
    public float undergroundPositionY;
    public int stage { get; private set; }
    public int world { get; private set; }
    public int lives { get; private set; }
    public int score { get; private set; }
    //public Canvas canvas;
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start()
    {
        NewGame();
        Time.timeScale = 1;
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void NewGame()
    {
        lives = 3;
        
        LoadLevel(1, 1);
    }

    private void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
        coinsList = new List<GameObject>();
        powShroomsList = new List<GameObject>();
        oneUpShroomsList = new List<GameObject>();
        starsList = new List<GameObject>();
        Time.timeScale = 1;
        //Invoke(nameof(TurnOffLoadingScene),2f);
    }
    
    public void ResetLevelWithDelay(float delay)
    {
        StartCoroutine(ResetLevelCourotine(delay));
    }

    private IEnumerator ResetLevelCourotine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ResetLevel();
    }

    public void ResetLevel()
    {
        lives--;
        if(lives > 0)
        {
            LoadLevel(world,stage);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        NewGame();
    }

    public void NextLevel()
    {
        if (stage <= 3)
            LoadLevel(world, ++stage);
        else
            LoadLevel(++world, 1);
    }

    public GameObject GetCoin(Vector3 position)
    {
        foreach (GameObject coin in coinsList)
        {
            if (!coin.activeInHierarchy)
            {
                coin.transform.position = position;
                return coin;
            }
        }
        GameObject cloneCoin = Instantiate(coin);
        cloneCoin.SetActive(false);
        cloneCoin.transform.position = position;
        coinsList.Add(cloneCoin);
        return cloneCoin;
    }
    public GameObject GetPowShroom(Vector3 position)
    {
        foreach (GameObject powShroom in powShroomsList)
        {
            if (!powShroom.activeInHierarchy)
            {
                powShroom.transform.position = position;
                return powShroom;
            }
        }
        GameObject clonePowShroom = Instantiate(powShroom);
        clonePowShroom.transform.position = position;
        clonePowShroom.SetActive(false);
        powShroomsList.Add(clonePowShroom);
        return clonePowShroom;
    }
    public GameObject GetOneUpShroom(Vector3 position)
    {
        foreach (GameObject oneUpShroom in oneUpShroomsList)
        {
            if (!oneUpShroom.activeInHierarchy)
            {
                oneUpShroom.transform.position = position;
                return oneUpShroom;
            }
        }
        GameObject cloneOneUpShroom = Instantiate(oneUpShroom);
        cloneOneUpShroom.transform.position = position;
        cloneOneUpShroom.SetActive(false);
        oneUpShroomsList.Add(cloneOneUpShroom);
        return cloneOneUpShroom;
    }
    public GameObject GetStar(Vector3 position)
    {
        foreach (GameObject star in starsList)
        {
            if (!star.activeInHierarchy)
            {
                star.transform.position = position;
                return star;
            }
        }
        GameObject cloneStar = Instantiate(star);
        cloneStar.transform.position = position;
        cloneStar.SetActive(false);
        starsList.Add(cloneStar);
        return cloneStar;
    }

    //private void TurnOffLoadingScene()
    //{
    //    canvas.GetComponent<Canvas>().enabled = false;
    //}
    public void AddLive()
    {
        lives++;
    }
}
