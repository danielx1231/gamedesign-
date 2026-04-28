using System;
using UnityEngine;
using UnityEngine.VFX;
using YuLongFSM;

namespace FSM.Playe
{
    public class Jump : FSMIState<FSMData>
    {
        Player player;
        HeroAnimations animations;
        Rigidbody2D rigidbody2D;
        SpriteRenderer spriteRenderer;

        [Header("2D��Ծ����")]
        float jumpForce = 14f;         // ��΢��߳�ʼ����������֤�߶Ȳ��䣩
        float moveSpeed = PlayerData.speed;
        int maxJumpCount = 2;

        // ��Ծ�������ؼ����������½����죩
        float riseGravity = 2.5f;    // ����ʱ���������� = ����������٣�
        float fallGravity = 4f;     // �½�ʱ���������� = ���ø��죩

        private float moveInput;
        private bool isGrounded;
        private int jumpCount;

        public override void OnEnter()
        {
            player = fSMData.creature as Player;
            spriteRenderer = fSMData.creature.spriteRenderer;
            rigidbody2D = fSMData.creature.rb;
            animations = fSMData.creature.animations;

            animations.PlayJump();
            animations.SetGround(false);

            jumpCount = 0;
            DoJump();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            moveInput = Input.GetAxis("Horizontal");
            isGrounded = fSMData.creature.IsGrounded();

            // ==========================================
            // ���ģ���̬�ı����� �� �����졢�½��졢�߶Ȳ���
            // ==========================================
            if (rigidbody2D.linearVelocity.y > 0)
            {
                // ������
                rigidbody2D.gravityScale = riseGravity;
            }
            else
            {
                // �½���
                rigidbody2D.gravityScale = fallGravity;
            }

            // ��� �� ��Idle
            if (isGrounded && rigidbody2D.linearVelocity.y <= 0)
            {
                rigidbody2D.gravityScale = 1; // �ָ���������
                animations.SetGround(true);
                fSMManager.Switch(FSMState.Idle);
                return;
            }

            Flip();

            // �����ƶ�
            Vector2 moveVel = new Vector2(moveInput * moveSpeed, rigidbody2D.linearVelocity.y);
            rigidbody2D.linearVelocity = moveVel;
        }

        void DoJump()
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpForce);
            jumpCount++;
            animations.PlayJump();
        }

        void Flip()
        {
            if (moveInput > 0.01f)
                spriteRenderer.flipX = false;
            else if (moveInput < -0.01f)
                spriteRenderer.flipX = true;
        }

        public override void OnExit()
        {
            jumpCount = 0;
            moveInput = 0;
            rigidbody2D.gravityScale = 1; // �˳�ʱ�ָ�����
        }
    }
}