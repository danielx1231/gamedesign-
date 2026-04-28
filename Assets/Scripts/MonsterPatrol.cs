using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterPatrol : MonoBehaviour
{
    [Header("Ѳ�ߵ�")]
    public Transform pointA;
    public Transform pointB;

    [Header("����")]
    public float moveSpeed = 1.8f;
    public float waitTime = 1f;
    public float arriveDis = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private Transform currentTarget;
    private float waitTimer;
    private bool isWaiting;

    // ��������
    private readonly string walkPara = "IsWalk";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentTarget = pointA;
    }

    void Update()
    {
        if (isWaiting)
        {
            WaitLogic();
        }
        else
        {
            PatrolMove();
        }
    }

    void PatrolMove()
    {
        Vector2 dir = (currentTarget.position - transform.position).normalized;

        // �ƶ�
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        // ��ת
        if (dir.x > 0.1f)
            sr.flipX = true;
        else if (dir.x < -0.1f)
            sr.flipX = false ;

        // ���߶���
        anim.SetBool(walkPara, true);

        // ����Ŀ��
        float dis = Vector2.Distance(transform.position, currentTarget.position);
        if (dis < arriveDis)
        {
            isWaiting = true;
            waitTimer = waitTime;
            rb.linearVelocity = Vector2.zero;
            // �д���
            anim.SetBool(walkPara, false);
        }
    }

    void WaitLogic()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            isWaiting = false;
            // �л�Ѳ�ߵ�
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    // ���� gizmos Ԥ��
    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag=="Player")
        {
            collision.transform.GetComponent<CapsuleCollider2D>().enabled = false;
            // ������ϵ�һ��
            collision.transform.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(rb.linearVelocity.x, 7f);
            collision.transform.GetComponent<Player>().enabled = false;
            collision.transform.GetComponent<HeroAnimations>().PlayDie();

            Timer.Instance.PlayTimer(5, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }
    internal void BeKilledByStep()
    {
        GameObject.Destroy(gameObject);
    }
}