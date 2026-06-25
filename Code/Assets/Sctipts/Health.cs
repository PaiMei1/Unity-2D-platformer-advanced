using System.Collections; 
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")] 
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

   
    private bool isInvulnerable = false; 

    [Header("iframes")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private float numberOffFlashes;
    private SpriteRenderer spriteRend;

    [SerializeField] private AudioClip deathSound; //
    [SerializeField] private AudioClip hurtSound;//
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>(); 
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if(currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerabilty());
            SoundManager.instance.PlaySound(hurtSound);//
        }
        else
        {
            if (!dead)
            {
               
                GetComponent<PlayerMovement>().enabled = false;
                anim.SetBool("Grounded", true);
                anim.SetTrigger("Death");
                dead = true;
                SoundManager.instance.PlaySound(deathSound); //
            }
        }

    }

    public void AddHealth( float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);

    }

    private IEnumerator Invunerabilty()
    {
        isInvulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11,true);
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/((numberOffFlashes)*2));     
            spriteRend.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(iFramesDuration / ((numberOffFlashes) * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        isInvulnerable = false;
    }
public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("Die");
        anim.Play("Idle");
        StartCoroutine(Invunerabilty());
        GetComponent<PlayerMovement>().enabled = true;
    }

}
