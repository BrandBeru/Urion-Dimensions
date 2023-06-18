using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public float damage;
    public float resistence;

    public float speed;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Image spr;
    [SerializeField] private Sprite normalFace;
    [SerializeField] private Sprite weakFace;
    [SerializeField] private Sprite happyFace;
    [SerializeField] private Sprite deadFace;
    [SerializeField] private Sprite winFace;

    public bool canMove;
    private void Start()
    {
        if(canMove)
            rb.velocity = Vector2.up*speed;
    }
    private void Update()
    {
        if (MiniGamesManager.instance.GetEnemyHealth() < 50)
            normalFace = weakFace;
        if (MiniGamesManager.instance.GetEnemyHealth() <= 1)
        {
            normalFace = deadFace;
            spr.sprite = deadFace;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Projectile")
        {
            StartCoroutine(Face(weakFace));
            MiniGamesManager.instance.TakeDamage(false);
        }
        if(collision.collider.tag == "Up")
            rb.velocity = Vector2.down * speed;
        else if(collision.collider.tag == "Down")
            rb.velocity = Vector2.up * speed;
    }
    private IEnumerator Face(Sprite changed)
    {
        spr.sprite = changed;
        yield return new WaitForSeconds(0.6f);
        spr.sprite = normalFace;

    }
}
