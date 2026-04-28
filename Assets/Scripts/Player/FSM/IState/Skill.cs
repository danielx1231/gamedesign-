using System;
using UnityEngine;
using YuLongFSM;

namespace FSM.Playe
{
    public class Skill : FSMIState<FSMData>
    {
        HeroAnimations animations;

        // 技能动画时长（根据你的动画修改）
        float skillDuration = 1f;
        float skillTimer;

        public override void OnEnter()
        {
            animations = fSMData.creature.animations;
            animations.PlaySkill();

            // 进入技能时重置计时
            skillTimer = skillDuration;
         //   AudioManager.Instance.PlayAudio("Sound/skill");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // 技能计时
            skillTimer -= Time.deltaTime;

            // 技能时间结束 → 自动回到Idle
            if (skillTimer <= 0)
            {
                fSMManager.Switch(FSMState.Idle);
                return;
            }

            // 技能期间可以跳跃打断
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }
        }

        public override void OnExit()
        {
            animations.SkillEnd();
        }
    }
}