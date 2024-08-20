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

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");

            //iframes
            StartCoroutine(Invulnerability());
        }
        else 
        {
            //player dead
            if(!dead)
            {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
            }
            
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
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
    }

    //public void Update() 
    //{ 
    //    if(Input.GetKeyDown(KeyCode.E) )
    //        TakeDamage(1);
    //}
}
