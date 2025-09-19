using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdMovement : MonoBehaviour
{
    [SerializeField] private Transform birdPath;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private Animator animator;

    private List<Transform> waypoints = new List<Transform>();
    private Transform bird;
    private Transform currentWaypoint;

    private float waitTime;

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

        float randomWait = waitTime + Random.Range(8, 16);

        // Mevcut waypoint dýþýnda rastgele seçim
        Transform target = GetNextWaypoint();

        // Kuþun yönünü belirle
        UpdateBirdDirection(target.position);

        currentWaypoint = target;

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

                Invoke(nameof(MoveToNextWaypoint), randomWait);
            });
    }

    private Transform GetNextWaypoint()
    {
        if (currentWaypoint == null)
            return waypoints[Random.Range(0, waypoints.Count)];

        List<Transform> possibleTargets = new List<Transform>(waypoints);
        possibleTargets.Remove(currentWaypoint); // ayný waypoint’i çýkar
        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }

    private void UpdateBirdDirection(Vector3 targetPos)
    {
        Vector3 direction = targetPos - bird.position;

        if (Mathf.Abs(direction.x) > 0.01f)
        {
            float newScaleX = Mathf.Sign(direction.x);
            Vector3 scale = bird.localScale;
            scale.x = newScaleX;
            bird.localScale = scale;
        }
    }
}
