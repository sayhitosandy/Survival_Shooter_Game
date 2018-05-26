using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    bool isInvincible = false;
    bool shieldCollected = false;
    float invincibilityTimer = 0f;
    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (invincibilityTimer >= 10f && shieldCollected)
        {
            shieldCollected = false;
            isInvincible = false;
        }

        if (shieldCollected)
        {
            invincibilityTimer += Time.deltaTime;
            //Debug.Log(invincibilityTimer);
        }
    }


    public void TakeDamage (int amount)
    {
        if (!isInvincible)
        {
            damaged = true;

            currentHealth -= amount;

            healthSlider.value = currentHealth;

            playerAudio.Play();

            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthCollectible"))
        {
            other.gameObject.SetActive(false);
            currentHealth += 50;
            if (currentHealth > startingHealth)
            {
                currentHealth = startingHealth;
            }

            healthSlider.value = currentHealth;
        }

        if (other.gameObject.CompareTag("ShieldCollectible"))
        {
            other.gameObject.SetActive(false);
            isInvincible = true;
            shieldCollected = true;
            invincibilityTimer = 0f;
        }
    }
}
