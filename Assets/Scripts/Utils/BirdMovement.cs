using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdMovement : MonoBehaviour
{
    [SerializeField] private Transform birdPath;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private float flipThreshold = 0.05f;

    private List<Transform> waypoints = new List<Transform>();
    private Transform bird;
    private Transform currentWaypoint;

    private void Start()
    {
        bird = transform;

        foreach (Transform child in birdPath)
            waypoints.Add(child);

        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        // Mevcut waypoint dýþýnda rastgele seçim
        Transform target;
        if (currentWaypoint == null)
        {
            target = waypoints[Random.Range(0, waypoints.Count)];
        }
        else
        {
            List<Transform> possibleTargets = new List<Transform>(waypoints);
            possibleTargets.Remove(currentWaypoint); // ayný waypoint’i çýkar
            target = possibleTargets[Random.Range(0, possibleTargets.Count)];
        }

        currentWaypoint = target;

        // Kuþun yönünü ayarla
        float deltaX = target.position.x - bird.position.x;
        if (Mathf.Abs(deltaX) > flipThreshold)
        {
            bird.localScale = new Vector3(Mathf.Sign(deltaX), 1f, 1f);
        }

        // Hareket
        bird.DOMove(target.position, moveDuration)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                if (animator != null)
                    animator.SetTrigger("IdleToFly");
            })
            .OnComplete(() =>
            {
                if (animator != null)
                    animator.SetTrigger("FlyToIdle");

                Invoke(nameof(MoveToNextWaypoint), waitTime);
            });
    }
}
