using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    public HeadMovement headMovement;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            headMovement.Activate();
        }
    }
}
