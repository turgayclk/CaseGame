using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private const float playerScale = 1.5f;

    private Transform cam;
    [SerializeField] Animator anim;

    bool isWalking;

    private void Start()
    {
        cam = Camera.main.transform; // ana kamerayý alýyoruz
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        float h = Input.GetAxisRaw("Horizontal"); // A-D
        float v = Input.GetAxisRaw("Vertical");   // W-S

        Vector3 input = new Vector3(h, 0f, v).normalized;

        // Walk animasyonu kontrolü
        if (input.magnitude == 1)
            isWalking = true;
        else
            isWalking = false;

        anim.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;

            Vector3 moveDir = camForward * v + camRight * h;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            if (h > 0)
                transform.localScale = new Vector3(playerScale, playerScale, playerScale);
            else if (h < 0)
                transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
        }
    }
}
