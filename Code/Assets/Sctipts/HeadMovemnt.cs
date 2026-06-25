using UnityEngine;
using System.Collections;

public class HeadMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float returnSpeed = 7f;

    [Header("Timing")]
    public float groundHitDelay = 0.3f;
    public float cooldownTime = 1.5f;

    private Vector3 startPosition;
    private bool isFalling = false;
    private bool isReturning = false;
    private bool canActivate = true;

    private Animator anim;
    [SerializeField] private AudioClip headDropSound;
private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isFalling)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
        else if (isReturning)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition,
                returnSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                isReturning = false;
                StartCoroutine(Cooldown());
            }
        }
    }

    public void Activate()
    {
        if (!canActivate) return;

        isFalling = true;
        canActivate = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling) return;

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Hit");
SoundManager.instance.PlaySound(headDropSound);
            StartCoroutine(ReturnRoutine());
        }
    }

    IEnumerator ReturnRoutine()
    {
        isFalling = false;

        yield return new WaitForSeconds(groundHitDelay);

        isReturning = true;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canActivate = true;
    }
}
