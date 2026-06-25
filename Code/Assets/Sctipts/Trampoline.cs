using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour
{
    [Header("Settings")]
    public float bounceForce = 12f;
    public float cooldownTime = 0.8f;

    private Animator animator;
    private bool canBounce = true;
    private bool isCoolingDown = false;
[SerializeField] private AudioClip trampolineSound;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Extended", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBounce || isCoolingDown) return;
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        StartCoroutine(BounceRoutine(rb));
    }

    private IEnumerator BounceRoutine(Rigidbody2D rb)
    {
        // LOCK trampoline
        canBounce = false;
        isCoolingDown = true;

        // Extend animation
        animator.SetBool("Extended", true);
SoundManager.instance.PlaySound(trampolineSound);

        // Small delay so animation visually extends
        yield return new WaitForSeconds(0.05f);

        // Apply bounce
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        // Wait for animation to finish
        yield return new WaitForSeconds(0.3f);

        // Retract
        animator.SetBool("Extended", false);

        // Cooldown
        yield return new WaitForSeconds(cooldownTime);

        // READY AGAIN
        canBounce = true;
        isCoolingDown = false;
    }
}
