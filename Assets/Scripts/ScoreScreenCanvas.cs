using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScoreScreenCanvas : MonoBehaviour
{
    Scene m_Scene;
    public Selectable selectedOnPopup;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    public float currentScore;

    public PlayerController2021Arena arena;

    private void Awake()
    {
        m_Scene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        InitializeUISelection();
    }

    void InitializeUISelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedOnPopup.gameObject);
    }

    public void ScoreDisplay()
    {
        scoreText.SetText("Your Score: " + currentScore.ToString());
        bestScoreText.SetText("Best Score: " + GameControl.control.ArenahighScore.ToString());
    }

    public void ScoreCheck()
    {
        if(currentScore > GameControl.control.ArenahighScore)
        {
            GameControl.control.ArenahighScore = currentScore;
        }
        ScoreDisplay();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(m_Scene.buildIndex);
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }
}
