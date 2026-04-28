using System;
using UnityEngine;
using YuLongFSM;

namespace FSM.Playe
{
    public class Attack2 : FSMIState<FSMData>
    {
        HeroAnimations animations;
        float attackDuration = 0.6f;
        float attackTimer;

        public override void OnEnter()
        {
            animations = fSMData.creature.animations;
            animations.PlayAttack2();
            attackTimer = attackDuration;
          //  AudioManager.Instance.PlayAudio("Sound/attack2");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            attackTimer -= Time.deltaTime;

            // 动画结束自动回Idle
            if (attackTimer <= 0)
            {
                fSMManager.Switch(FSMState.Idle);
                return;
            }

            // 打断
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                fSMManager.Switch(FSMState.Skill);
            }
        }

        public override void OnExit()
        {
            animations.Attack2End();
        }
    }
}