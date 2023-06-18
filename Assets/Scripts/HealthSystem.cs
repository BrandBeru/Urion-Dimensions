using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public bool isDeath = false;

    public Animator anim;
    public ParticleSystem ps;

    public int experienceAmount;
    
    public float health;
    public float maxHealth;


    public void TakeDamage(float amount)
    {
        health -= amount;
        PlayerPrefs.SetFloat(gameObject.tag + "_health", health);
        GameManager.instance.UpdateView();
        anim.SetTrigger("Hurt");

        if (health <= 0)
            StartDied();
    }
    public void TakeHealth(int amount)
    {
        health += amount;
        PlayerPrefs.SetFloat(gameObject.tag + "_health", health);
        anim.SetTrigger("Hurt");

        if (health >= 0)
            health = maxHealth;
    }
    public void StartDied()
    {
        StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        Debug.Log("Died!");
        anim.SetBool("IsDeath", true);
        //IsEnemy
        if(gameObject.tag == "Enemy" && !isDeath)
        {
            experienceAmount += GameManager.instance.experience;
            isDeath = true;
            PlayerPrefs.SetInt("EXPERIENCE", experienceAmount);
            GameManager.instance.UpdateView();
        }
        //particles system
        ps.transform.position = transform.position;
        ps.Play();
        yield return new WaitForSeconds(2);
        ps.Stop();
        Destroy(gameObject);
    }
}
