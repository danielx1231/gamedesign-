using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public NavMeshAgent agent;
    public CharacterController characterController;
    public Rigidbody2D rb;
    public HeroAnimations animations;

    public RoleData roleData;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
        animations = gameObject.AddComponent<HeroAnimations>();
    }

    public virtual void Hurt(int attack)
    {
        roleData.hp -= attack;
        if (roleData.hp <= 0)
        {
            this.enabled = false;
            animations.PlayDie();
        }
    }
    public void TurnTo(Vector3 direction)//形参填入需要朝向的方向的向量
    {
        if (direction == Vector3.zero)
        {
            return;
        }
        Quaternion q = Quaternion.identity;
        q.SetLookRotation(direction);//setlookrotaion定义看向指定方向的rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, q.eulerAngles.y, 0), Time.deltaTime * 10);
    }

    [Header("地面检测")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
