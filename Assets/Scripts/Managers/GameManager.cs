using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Button restartButton;
    [SerializeField] Button reviveButton;

    private bool gameOver = false;

    public bool IsGameOver => gameOver;

    private void OnEnable()
    {
        Health.OnDeath += HandleDeath;
    }

    private void HandleDeath()
    {
        gameOver = true;
    }

    private void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void OnSceneLoaded()
    {
        // RestartButton butonunu sahnede bul
        GameObject buttonObj = GameObject.Find("RestartButton");
        GameObject reviveObj = GameObject.Find("ReviveButton");
        if (buttonObj != null)
        {
            restartButton = buttonObj.GetComponent<Button>();
            restartButton.onClick.RemoveAllListeners(); // ayný listener tekrar eklenmesin
            restartButton.onClick.AddListener(RestartGame);
        }

        if (reviveObj != null)
        {
            reviveButton = reviveObj.GetComponent<Button>();
            reviveButton.onClick.RemoveAllListeners(); // ayný listener tekrar eklenmesin
            reviveButton.onClick.AddListener(RevivePlayer);
        }
    }

    public void RevivePlayer()
    {
        gameOver = false;
        Time.timeScale = 1f;

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Revive();
        }
        Debug.Log("Player revived!");
    }

    public void RestartGame()
    {
        gameOver = false;

        Time.timeScale = 1f;
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Level restarted!");
    }
}
