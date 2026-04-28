using FSM.Playe;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using YuLongFSM;

public class Player : Creature
{
    public static Player Instance;
    public Transform shou;
    public FSMManager<FSMData> fSM;
    FSMData fSMData = new FSMData();
    private Vector3 bulletPos;
    private float hp = 100;
    bool isDie;

    private void Awake()
    {
        Instance = this;

        fSM = new FSMManager<FSMData>(fSMData);
        fSMData.creature = this;

        // ע��ԭ��״̬
        fSM.Register(FSMState.Idle, new Idle());
        fSM.Register(FSMState.Walk, new Walk());
        fSM.Register(FSMState.Climb, new Climb());


        fSM.Register(FSMState.Jump, new Jump());


   
        //����Ѫ���¼�
      //  WindowEvent.Instance.AddEventListener(WindowEventType.AddHp, AddHp);
    }
    public void OnDestroy()
    {
       // WindowEvent.Instance.RemoveEventListener(WindowEventType.AddHp, AddHp);
    }
    private void AddHp(object obj)
    {
        float vaule = 0;
        float.TryParse(obj.ToString(), out vaule);
        roleData.hp += (int)vaule;
        if (roleData.hp>roleData.maxHp)
        {
            roleData.hp = roleData.maxHp;
        }
    }



    protected override void Start()
    {
        base.Start();

        fSM.Switch(FSMState.Idle);
    }

    float jieTime;
    void Update()
    {
        if (fSM != null)
        {
            fSM.OnUpdate();
        }

        CheckStepMonster();

        if (fSMData.creature.IsGrounded()&& isTan)
        {
            //  ��̤��ֱ����������������������
           rb.gravityScale = 1f;
            isTan = false;
        }
    }

    public override void Hurt(int attack)
    {
        base.Hurt(attack);

    }

    public void Gravity(float gravity = 5)
    {
        characterController.Move(Vector3.down * Time.deltaTime * gravity);
    }
    bool isTan;
    void CheckStepMonster()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            new Vector2(groundCheck.position.x, groundCheck.position.y),
            groundCheckRadius);

        foreach (var hit in hits)
        {
            MonsterPatrol monster = hit.GetComponent<MonsterPatrol>();
            if (monster != null)
            {
                // ��̤���ϵ�
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 14f);

                //  ��̤��ֱ����������������������
                rb.gravityScale = 4f;
                isTan = true;
                // ��������
                monster.BeKilledByStep();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);

    }
}