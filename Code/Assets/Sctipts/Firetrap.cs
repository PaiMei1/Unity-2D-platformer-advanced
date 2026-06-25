using UnityEngine;
using System.Collections;
public class Firetrap : MonoBehaviour
{
    [SerializeField] private int damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    [SerializeField] private AudioClip firetrapSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());

            }
            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);

        }

    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(activationDelay); //sacekaj onoliko sekundi koliko se nalazi u activation delay    
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("Activated", true);
        SoundManager.instance.PlaySound(firetrapSound);
        yield return new WaitForSeconds(activeTime);//sacekaj onoliko sekundi koliko se nalazi u active time
        active = false;
        triggered = false;
        anim.SetBool("Activated", false);
    }
}
