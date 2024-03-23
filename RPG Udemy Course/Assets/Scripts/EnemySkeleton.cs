using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{

    private bool _isAttacking;

    [Header("Move Info")]
    [SerializeField] private float _moveSpeed;

    [Header("Player Detection")]
    [SerializeField] private float _playerCheckDistance;
    [SerializeField] private LayerMask _whatIsPlayer;
    private RaycastHit2D _isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

        if (_isPlayerDetected)
        {
            if (_isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(_moveSpeed * 6.0f * facingDirection, rb.velocity.y);

                Debug.Log("I see the player");
                _isAttacking = false;
            }
            else
            {
                Debug.Log("ATTACK! " + _isPlayerDetected.collider.gameObject.name);
                _isAttacking = true;
            }
        }
        else
        {
            Movement();
        }

        if (!_isGrounded || _isWallDetected)
        {
            Flip();
        }

    }

    private void Movement()
    {
        if(!_isAttacking)
        {
            rb.velocity = new Vector2(_moveSpeed * facingDirection, rb.velocity.y);
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        _isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, _playerCheckDistance * facingDirection, _whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + _playerCheckDistance * facingDirection, transform.position.y));
    }
}
