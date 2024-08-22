using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOffFlashes;
    private SpriteRenderer spriteRenderer;

    [Header("Components")]
    [SerializeField] Behaviour[] components;
    private bool invulnerable;

    [Header("Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");

            //iframes
            StartCoroutine(Invulnerability());
            
            SoundManager.instance.PlaySound(hurtSound);
        }
        else 
        {
            //player dead
            if(!dead)
            {
                

                //if(GetComponent<PlayerMovement>() != null)
                //    GetComponent<PlayerMovement>().enabled = false;

                //if(GetComponentInParent<EnemyPatrol>() != null)
                //    GetComponentInParent<EnemyPatrol>().enabled = false;

                //if(GetComponent<MeleeEnemy>() != null)
                //    GetComponent<MeleeEnemy>().enabled = false;
                
                foreach (Behaviour comp in components)
                {
                    comp.enabled = false;
                }
                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
            
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("idle");
        StartCoroutine(Invulnerability());
        foreach (Behaviour comp in components)
        {
            comp.enabled = true;
        }
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8,9, true);
        //invulnerable
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration/(numberOffFlashes*2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOffFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
        invulnerable = false;
    }

    //public void Update() 
    //{ 
    //    if(Input.GetKeyDown(KeyCode.E) )
    //        TakeDamage(1);
    //}
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
