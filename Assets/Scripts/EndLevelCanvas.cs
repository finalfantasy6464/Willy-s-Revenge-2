using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndLevelCanvas : MonoBehaviour
{
    Scene m_Scene;
    public Button retryButton;
    public GameObject nextLevelButton;
    public CanvasGroup canvasGroup;

    public Sprite[] emblemsprites;
    public Image completeImage;
    public Image goldenImage;
    public Image timerImage;

    private void Awake()
    {
        m_Scene = SceneManager.GetActiveScene();
    }

    private void Start()
    {
        nextLevelButton.SetActive(!(m_Scene.buildIndex % 10.0f == 0));
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(m_Scene.buildIndex);
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(m_Scene.buildIndex + 1);
    }

    public void UpdateEmblemStatus()
    {
        completeImage.sprite = GameControl.control.completedlevels[GameControl.control.levelID] ? emblemsprites[1] : emblemsprites[0];
        goldenImage.sprite = GameControl.control.goldenpellets[GameControl.control.levelID] ? emblemsprites[2] : emblemsprites[0];
        timerImage.sprite = GameControl.control.timerchallenge[GameControl.control.levelID] ? emblemsprites[3] : emblemsprites[0];
    }

    public void FocusRetryButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(retryButton.gameObject);
    }
}
