using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlastmaticGame_manager : MonoBehaviour
{
    [SerializeField] Rigidbody2D ball;
    [SerializeField] float initialBallVelocity = 4f;

    private float velAmount;
    private Vector2 initPos;

    private void Start()
    {
        Vector2 enemyPos = MiniGamesManager.instance.enemy.transform.position;
        initPos = new Vector2(enemyPos.x - 0.2f, enemyPos.y);
        Launch();
    }
    private void Launch()
    {
        ball.velocity = new Vector2(-1, -1) * initialBallVelocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            MiniGamesManager.instance.PointAmount(true);
            StartCoroutine(ResetPosition());
        }
        else if (collision.collider.tag == "Enemy")
        {
            MiniGamesManager.instance.PointAmount(false);
            StartCoroutine(ResetPosition());
        }
        if (collision.collider.tag == "PlayerCorner" || collision.collider.tag == "EnemyCorner")
        {
            velAmount = Mathf.Log10(Mathf.Sqrt(initialBallVelocity)*2);
            initialBallVelocity +=velAmount;
            MiniGamesManager.instance.enemy.speed += velAmount;
            MiniGamesManager.instance.player.speed += velAmount / 2;

            if (initialBallVelocity >= 30)
                MiniGamesManager.instance.GamePass();
        }
    }
    private IEnumerator ResetPosition()
    {
        Vector2 enemyPos = MiniGamesManager.instance.enemy.transform.position;
        initPos = new Vector2(enemyPos.x - 0.5f, enemyPos.y);
        transform.position = initPos;
        ball.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        Launch();
    }
}
