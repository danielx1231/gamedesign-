using System;
using UnityEngine;
using YuLongFSM;

namespace FSM.Playe
{
    public class Hit : FSMIState<FSMData>
    {
        HeroAnimations animations;

        // 技能动画时长（根据你的动画修改）
        float hit = 0.5f;
        float hitTime;

        public override void OnEnter()
        {
            animations = fSMData.creature.animations;
            animations.PlayGethit();

            // 进入技能时重置计时
            hitTime = hit;
        //    AudioManager.Instance.PlayAudio("Sound/hit");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // 技能计时
            hitTime -= Time.deltaTime;

            // 技能时间结束 → 自动回到Idle
            if (hitTime <= 0)
            {
                fSMManager.Switch(FSMState.Idle);
                return;
            }

        }

        public override void OnExit()
        {
            animations.GethitEnd();
        }
    }
}