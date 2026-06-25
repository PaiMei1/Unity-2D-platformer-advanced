using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float pushSpeed = 5f;
    [SerializeField] private Vector2 fanDirection = Vector2.right;

    private Rigidbody2D playerRb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRb = collision.attachedRigidbody;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRb = null;
        }
    }

    private void FixedUpdate()
    {
        if (playerRb == null) return;

        Vector2 dir = fanDirection.normalized;

        // MovePosition cannot be canceled by velocity changes
        Vector2 newPos = playerRb.position + dir * pushSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(newPos);
    }
}
