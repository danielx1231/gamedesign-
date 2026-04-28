using UnityEngine;
using YuLongFSM;
using static HeroAnimations;

namespace FSM.Playe
{
    public class Climb : FSMIState<FSMData>
    {
        Rigidbody2D rigidbody2D;
        HeroAnimations animations;
        SpriteRenderer spriteRenderer;

        private float climbSpeed = 2.5f;
        private float checkRadius = 0.4f;

        public override void OnEnter()
        {
            spriteRenderer = fSMData.creature.spriteRenderer;
            rigidbody2D = fSMData.creature.rb;
            animations = fSMData.creature.animations;

            animations.PlayClimbIdle();
            rigidbody2D.gravityScale = 0;
            rigidbody2D.linearVelocity = Vector2.zero;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            bool isInClimbArea = CheckClimbAround();

            if (!isInClimbArea)
            {
                ExitClimb();
                fSMManager.Switch(FSMState.Idle);
                return;
            }

            ClimbMove();
        }

        private bool CheckClimbAround()
        {
            Vector2 checkPos = rigidbody2D.transform.position;
            Collider2D[] hits = Physics2D.OverlapCircleAll(checkPos, 0.4f);

            foreach (var item in hits)
            {
                if (item.transform.CompareTag("Ladder"))
                {
                    return true;
                }
            }
            return false;
        }

        private void ClimbMove()
        {
            //  ���� X �����루���ң�
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            // ���ҷ�ת������Walk���߼�һ����
            if (x > 0.1f)
                spriteRenderer.flipX = false;
            else if (x < -0.1f)
                spriteRenderer.flipX = true;

            //  idle ����
            if (x == 0 && y == 0)
            {
                animations.PlayClimbIdle();
            }
            else
            {
                animations.PlayClimbMove();
            }

            //  �ؼ���ͬʱ֧�� X + Y �ƶ�������+���£�
            rigidbody2D.linearVelocity = new Vector2(x * climbSpeed, y * climbSpeed);
        }

        private void ExitClimb()
        {
            animations.PlayClimbEnd();
            rigidbody2D.gravityScale = 1;
        }

        public override void OnExit()
        {
            base.OnExit();
            ExitClimb();
        }
    }
}