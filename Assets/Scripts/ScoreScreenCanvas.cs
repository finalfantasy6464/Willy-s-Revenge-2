using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenCanvas : MonoBehaviour
{
    Scene m_Scene;

    public Text TitleText;
    public Text scoreText;
    public Text bestScoreText;

    public float currentScore;

    public PlayerController2021Arena arena;

    private void Awake()
    {
        m_Scene = SceneManager.GetActiveScene();
    }

    public void ScoreDisplay()
    {
        scoreText.text = "Your Score: " + currentScore.ToString();
        bestScoreText.text = "Best Score: " + GameControl.control.ArenahighScore.ToString();
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
