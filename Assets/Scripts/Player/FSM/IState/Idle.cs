using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YuLongFSM;

namespace FSM.Playe
{
    public class Idle : FSMIState<FSMData>
    {
        HeroAnimations animations;
        private Vector3 direction;
        bool isMove;
        private Vector3 forward;
        SpriteRenderer spriteRenderer;
        float idleTime;
        public override void OnEnter()
        {
            spriteRenderer = fSMData.creature.spriteRenderer;
            animations = fSMData.creature.animations;
            animations.PlayIdle();
            fSMData.creature.GetComponent<Animator>().SetFloat("IdleInt", 0);
            idleTime = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            if (animations.state!=HeroAnimations.HState.Idle)
            {
                animations.PlayIdle();
            }
            if (direction!=Vector3.zero)
            {
                isMove = true;
                fSMManager.Switch(FSMState.Walk);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    fSMManager.Switch(FSMState.Run);
                    return;
                }
                else
                {
                    fSMManager.Switch(FSMState.Walk);
                    return;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }

        }


  
    }
}
