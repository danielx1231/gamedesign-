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
    public class Run : FSMIState<FSMData>
    {
        CharacterController characterController;
        Rigidbody2D rigidbody2D;
        HeroAnimations animations;
        SpriteRenderer spriteRenderer;
        public override void OnEnter()
        {
            spriteRenderer = fSMData.creature.spriteRenderer;
            rigidbody2D = fSMData.creature.rb;
            animations = fSMData.creature.animations;
            animations.PlayRun();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            Move();

        }
        public void Move()
        {
            float x = Input.GetAxis("Horizontal");

            if (x > 0.1f)
                spriteRenderer.flipX = false;
            else if (x < -0.1f)
                spriteRenderer.flipX = true;

            if (x != 0)
            {
                rigidbody2D.linearVelocity = new Vector2(x * PlayerData.speed*2, rigidbody2D.linearVelocity.y);

            }
            else
            {
                fSMManager.Switch(FSMState.Idle);

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                fSMManager.Switch(FSMState.Walk);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                fSMManager.Switch(FSMState.Attack);
            }
        }
    }
    
}
