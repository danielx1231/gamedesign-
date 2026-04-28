using System;
using UnityEngine;
using YuLongFSM;

namespace FSM.Playe
{
    public class Attack : FSMIState<FSMData>
    {
        HeroAnimations animations;
        Rigidbody2D rb;
        Transform creatureTrans;

        // ��������
        float attackDuration = 0.6f;
        float attackTimer;
        bool canAttack;

        public override void OnEnter()
        {
            // ��ȡ���
            animations = fSMData.creature.animations;
            rb = fSMData.creature.GetComponent<Rigidbody2D>();
            creatureTrans = fSMData.creature.transform;

            // ���Ź�������
            animations.PlayAttack();

            // ������ʱ��
            attackTimer = attackDuration;
            canAttack = true;

            // ������Ч
        //    AudioManager.Instance.PlayAudio("Sound/attack2");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            attackTimer -= Time.deltaTime;

            // ==========================================
            // 2D ���ģ�����ʱֹͣ�ƶ��������泯����
            // ==========================================
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }

            // ==========================================
            // ������������ �� �ص�Idle
            // ==========================================
            if (attackTimer <= 0 && canAttack)
            {
                canAttack = false;
                fSMManager.Switch(FSMState.Idle);
                return;
            }

            // ==========================================
            // ������������ϣ���Ծ/���ܣ�
            // ==========================================
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }
        }

        public override void OnExit()
        {
            animations.AttackEnd();
        }
    }
}