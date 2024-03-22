using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{

    [Header("Move Info")]
    [SerializeField] private float _moveSpeed;

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

        if(!_isGrounded)
        {
            Flip();
        }

        rb.velocity = new Vector2(_moveSpeed * facingDirection, rb.velocity.y);
    }
}
