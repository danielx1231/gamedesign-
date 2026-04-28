using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YuLongFSM;
using static HeroAnimations;

namespace FSM.Playe
{
    public class Walk : FSMIState<FSMData>
    {
        Rigidbody2D rigidbody2D;
        HeroAnimations animations;
        SpriteRenderer spriteRenderer;
        public override void OnEnter()
        {
            spriteRenderer = fSMData.creature.spriteRenderer;
            rigidbody2D = fSMData.creature.rb;
            animations= fSMData.creature.animations; 
            animations.PlayWalk();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            Move();
            
        }
        // 2D 球形重叠检测
        private bool CheckClimbAround()
        {
            Vector2 checkPos = rigidbody2D.transform.position;
            // 球形检测
            Collider2D[] hits = Physics2D.OverlapCircleAll(checkPos, 0.4f);

            foreach (var item in hits)
            {
                if (item.transform.tag == "Ladder")
                {
                    return true;
                }
            }

            return false;
        }
        public  void Move()
        {
         float x = Input.GetAxis("Horizontal");


            if (x > 0.1f)
                spriteRenderer.flipX = false ;
            else if (x < -0.1f)
                spriteRenderer.flipX = true ;

            if (x != 0)
            {
                rigidbody2D.linearVelocity = new Vector2(x *PlayerData.speed , rigidbody2D.linearVelocity.y);

            }
            else
            {
                fSMManager.Switch(FSMState.Idle);

            }

            if (CheckClimbAround())
            {
                fSMManager.Switch(FSMState.Climb);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                fSMManager.Switch(FSMState.Run);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                fSMManager.Switch(FSMState.Attack);
            }
        }
    }
}
