using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMode : HealthSystem
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;

    public float attackRate = 2f;
    private float nextAttackTime = 0;

    public float hitImpact = 2f;

    public float powerHit;
    public LayerMask enemyLayers;

    public void Attack()
    {
        if(Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyController>().TakeDamage(powerHit);
                if(enemy.GetComponent<Transform>().localScale.x == -1)
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
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
