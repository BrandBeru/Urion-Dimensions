using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool onPlayerCollision;
    public bool isPatrol;

    private bool isFacingRight;
    private float horizontal;

    private void Start()
    {
        nextPosIndex = 0;
        nextPoint = points[0];

        onPlayerCollision = false;
        health = maxHealth;
        ps.Pause();
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void Update()
    {
        if (onPlayerCollision && !isDeath)
            Attack();
        if (isPatrol && !onPlayerCollision && !isDeath)
        {
            NextPoint();
            anim.SetTrigger("IsPatrol");
        }
        Flip();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            onPlayerCollision= true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            onPlayerCollision = false;
        }
    }

    public Transform[] points;
    public Transform nextPoint;

    public Transform player;
    public int nextPosIndex;

    public int speed;

    public void NextPoint()
    {
        if (transform.position.x == nextPoint.position.x)
        {
            nextPosIndex++;
            if (nextPosIndex >= points.Length)
                nextPosIndex = 0;
            nextPoint = points[nextPosIndex];
        }
        else
        {
            if (nextPosIndex == 1)
                horizontal = -1;
            else
                horizontal = 1;
            transform.position = Vector3.MoveTowards(transform.position, nextPoint.position, speed * Time.deltaTime);
        }
    }
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;

    public float attackRate = 2f;
    private float nextAttackTime = 0;

    public float hitImpact = 2f;

    public float powerHit = 20;
    public LayerMask enemyLayers;

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<HealthSystem>().TakeDamage(powerHit);
                if (enemy.GetComponent<Transform>().localScale.x == -1)
                    enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.left * hitImpact, ForceMode2D.Impulse);
                else
                    enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.right * hitImpact, ForceMode2D.Impulse);
                Debug.Log("We Hit " + enemy.name);
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public float maxHealth;
    public float health;
    public Animator anim;

    public bool isDeath = false;

    public ParticleSystem ps;

    public int experienceAmount;

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
        if (gameObject.tag == "Enemy" && !isDeath)
        {
            isDeath = true;
            GameManager.instance.experience += experienceAmount;
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
