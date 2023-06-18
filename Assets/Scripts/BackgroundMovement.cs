using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] materials;
    public Vector2 offset;

    public float minus = 0.5f;

    public Rigidbody2D follow;

    private void Update()
    {
        offset = new Vector2((follow.velocity.x * Time.deltaTime) * minus, 0);
        foreach(SpriteRenderer material in materials)
            material.material.mainTextureOffset += offset;
    }
}
