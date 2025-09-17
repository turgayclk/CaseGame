using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;

    [Header("Ambient Sounds")]
    [SerializeField] private AudioSource birdSource;
    [SerializeField] private AudioSource windSource;

    [Header("Music Lists")]
    [SerializeField] private List<AudioClip> IdleMusics = new List<AudioClip>();
    [SerializeField] private List<AudioClip> WaveMusics = new List<AudioClip>();

    private AudioClip currentClip;
    private bool isWaveMusicPlaying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        EnemyManager.OnEnemyCountChanged += HandleMusic;
    }

    private void OnDisable()
    {
        EnemyManager.OnEnemyCountChanged -= HandleMusic;
    }

    public void HandleMusic()
    {
        bool hasEnemy = EnemyManager.Instance.GetActiveEnemies().Count > 0;

        if (hasEnemy && !isWaveMusicPlaying)
        {
            // düþman varsa ama wave müziði çalmýyorsa ? wave baþlat
            PlayRandomFromList(WaveMusics);

            // kuþ seslerini durdur
            birdSource.Stop();

            // rüzgar seslerini durdur
            windSource.Stop();

            isWaveMusicPlaying = true;
        }
        else if (!hasEnemy && isWaveMusicPlaying)
        {
            // düþman kalmadýysa ? idle baþlat
            PlayRandomFromList(IdleMusics);

            // kuþ seslerini baþlat
            birdSource.Play();

            // rüzgar seslerini baþlat
            windSource.Play();

            isWaveMusicPlaying = false;
        }
    }

    private void PlayRandomFromList(List<AudioClip> list)
    {
        if (list.Count == 0) return;

        AudioClip newClip = list[Random.Range(0, list.Count)];

        if (newClip == currentClip) return; // ayný þarkýyý tekrar baþlatma

        currentClip = newClip;
        musicSource.clip = currentClip;
        musicSource.Play();
    }
}
