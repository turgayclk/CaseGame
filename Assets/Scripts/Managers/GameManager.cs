using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Button restartButton;

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
        // TryAgain butonunu sahnede bul
        GameObject buttonObj = GameObject.Find("TryAgain");
        if (buttonObj != null)
        {
            restartButton = buttonObj.GetComponent<Button>();
            restartButton.onClick.RemoveAllListeners(); // ayný listener tekrar eklenmesin
            restartButton.onClick.AddListener(RestartWave);
        }
    }

    public void RestartWave()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Level restarted!");
    }
}
