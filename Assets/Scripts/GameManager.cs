using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textLevel;
    [SerializeField] private  GameObject panelWin;
    [SerializeField] private  GameObject panelLose;
    [SerializeField] private  int LevelToWin;
    private int currentLevel =0;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void Start()
    {
        
        SetTextLevel (currentLevel);
    }
    private void Update()
    {
        if(currentLevel>= LevelToWin)
        {
            GameWin();
        }
    }

    void SetTextLevel(int level)
    {
        textLevel.text ="Nivel: " + level.ToString();
    }
    void AddLevel(int level)
    {
        currentLevel += level;
        SetTextLevel(currentLevel);
        
    }

    void GameWin()
    {
        panelWin.SetActive (true);
      
        Time.timeScale = 0;
    }


    void GameLose()
    {
        panelLose.SetActive (true);   
        
        Time .timeScale = 0;
    }


    public void OnClickRestart()
    {
        
        SceneManager.LoadScene("Game");
      

    }

    private void OnEnable()
    {
        HealthBarController.OnPlayerDeath += GameLose;
        HealthBarController.OnDestroyEnemie1 += AddLevel;
        HealthBarController.OnDestroyEnemie2 += AddLevel;


    }


    void OnDisable()
    {
        HealthBarController.OnPlayerDeath -= GameLose;
        HealthBarController.OnDestroyEnemie1 -= AddLevel;
        HealthBarController.OnDestroyEnemie2 -= AddLevel;

    }

}
