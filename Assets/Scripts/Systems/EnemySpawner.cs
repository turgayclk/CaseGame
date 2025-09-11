using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Transform[] pathPoints;
    [SerializeField] private WaveSystem waveSystem;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI waveKeyInfo;   // Space bas yazýsý
    [SerializeField] private TextMeshProUGUI waveStartText; // Dalga baþladýðýnda çýkan yazý
    [SerializeField] private TextMeshProUGUI nextWaveInfoText; // sonraki wave bilgisi
    [SerializeField] private GameObject nextWaveInfoBG; // sonraki wave bilgisi

    private int currentWaveIndex = 0;
    private Tween waveInfoTween;
    private string lastWaveText; 

    private void OnEnable()
    {
        Health.OnDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= PlayerDeath;
    }

    private void Start()
    {
        ShowWaveInfo(); // Baþta gösterilsin
        if (waveStartText != null)
            waveStartText.gameObject.SetActive(false); // baþta kapalý
        if (nextWaveInfoText != null)
            nextWaveInfoText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryStartNextWave();
        }
    }

    private void TryStartNextWave()
    {
        // GenerateWave kullanarak wave üret
        Wave wave = waveSystem.GenerateWave(currentWaveIndex);

        HideWaveInfo();
        WaveRoutine(wave);
    }

    private void WaveRoutine(Wave wave)
    {
        StartCoroutine(WaveRoutineCoroutine(wave));
    }

    private IEnumerator WaveRoutineCoroutine(Wave wave)
    {
        // Wave baþlama yazýsý
        ShowWaveStartText($"{wave.waveName}");

        // Sonraki wave varsa bilgiyi göster
        Wave nextWave = waveSystem.GenerateWave(currentWaveIndex + 1);
        ShowNextWaveInfo(nextWave);

        // Düþmanlarý spawn et
        yield return StartCoroutine(SpawnWave(wave));

        // Wave tamamlandý
        currentWaveIndex++;

        // Wave info tekrar gösterilsin, space ile baþlatýlabilir olsun
        ShowWaveInfo();
    }

    private void PlayerDeath()
    {
        waveKeyInfo.gameObject.SetActive(false);
        waveStartText.gameObject.SetActive(false);
        nextWaveInfoText.gameObject.SetActive(false);
        nextWaveInfoBG.gameObject.SetActive(false);
    }

    // waveStartText animasyonu gibi
    private void ShowNextWaveInfo(Wave nextWave)
    {
        if (nextWaveInfoText == null || nextWave == null) return;

        nextWaveInfoText.gameObject.SetActive(true);

        // Texti oluþtur
        string text = $"{nextWave.waveName} - ";
        bool first = true;

        foreach (var enemy in nextWave.enemies)
        {
            if (enemy.count <= 0) continue; // 0 olanlarý atla

            if (!first) text += " + ";
            text += $"{enemy.enemyType.name}({enemy.count})";
            first = false;
        }

        nextWaveInfoText.text = text;

        // Önce varsa eski animasyonu durdur
        nextWaveInfoText.rectTransform.DOKill();

        // Hafif titreme animasyonu
        nextWaveInfoText.rectTransform.DOShakeScale(
            duration: 0.5f,
            strength: 0.1f,
            vibrato: 5,
            randomness: 90,
            fadeOut: true
        );
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        foreach (var waveEnemy in wave.enemies)
        {
            for (int i = 0; i < waveEnemy.count; i++)
            {
                SpawnEnemy(waveEnemy.enemyType);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }
    }

    private void SpawnEnemy(EnemyType type)
    {
        if (type == null || type.prefab == null)
        {
            Debug.LogWarning("EnemyType eksik!");
            return;
        }

        GameObject enemy = Instantiate(type.prefab, pathPoints[0].position, type.prefab.transform.rotation);

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Initialize(type, pathPoints);
        }
    }

    private void ShowWaveInfo()
    {
        if (waveKeyInfo == null) return;

        waveKeyInfo.gameObject.SetActive(true);
        waveInfoTween?.Kill();

        waveInfoTween = DOTween.Sequence()
            .Join(waveKeyInfo.rectTransform.DOScale(1.1f, 0.8f).SetLoops(-1, LoopType.Yoyo));
    }

    private void HideWaveInfo()
    {
        if (waveKeyInfo == null) return;

        waveInfoTween?.Kill();
        waveKeyInfo.gameObject.SetActive(false);
        waveKeyInfo.rectTransform.localScale = Vector3.one;
        waveKeyInfo.alpha = 1f;
    }

    private void ShowWaveStartText(string text)
    {
        if (waveStartText == null) return;

        // Eðer text deðiþtiyse animasyonu baþtan baþlat
        if (lastWaveText != text)
        {
            lastWaveText = text;

            // Mevcut animasyonlarý durdur
            waveStartText.DOKill();

            waveStartText.text = text;
            waveStartText.gameObject.SetActive(true);
            waveStartText.alpha = 0f;

            // FadeIn ? bekle ? FadeOut
            waveStartText.DOFade(1f, 0.8f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    waveStartText.DOFade(0f, 0.8f).OnComplete(() =>
                    {
                        waveStartText.gameObject.SetActive(false);
                        lastWaveText = null; // bitince sýfýrla ki sonraki text tekrar baþlatýlabilsin
                    });
                });
            });
        }
    }

}
