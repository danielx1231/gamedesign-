using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimations : MonoBehaviour
{
    public enum HState
    {
        None,
        Idle,
        Walk,
        Run,
        Attack,
        Attack2, // 二段攻击
        Skill,   // 技能
        Jump,    // 跳跃
        Die,
        Gethit,
        Intonate,
        switchover,
        WaKuang,
        KanSHu,
        Climb,
    }

    public HState state = HState.Idle;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void SetFalseAll()
    {
        animator.SetBool("idle", false);
        animator.SetBool("run", false);
        animator.SetBool("walk", false);
        animator.SetBool("die", false);
    }

    // ========== 原有方法保留 ==========
    public void PlayIdle()
    {
        state = HState.Idle;
        SetFalseAll();
        animator.SetBool("idle", true);

    }

    public void PlayRun()
    {
        if (state == HState.Attack || state == HState.Attack2 || state == HState.Skill || state == HState.Jump)
            return;
        SetFalseAll();
        animator.SetBool("run", true);
        state = HState.Run;
    }

    public void PlayWalk()
    {
        if (state == HState.Attack || state == HState.Attack2 || state == HState.Skill || state == HState.Jump)
            return;
        SetFalseAll();
        animator.SetBool("walk", true);
        state = HState.Walk;
    }

    public void PlayAttack()
    {
        SetFalseAll();
        animator.SetTrigger("attack1"); // 改用Trigger避免动画重复触发
        state = HState.Attack;
    }

    public void PlayAttack1()
    {
        SetFalseAll();
        animator.SetTrigger("attack1");
        state = HState.Attack;
    }

    public void PlayDie()
    {
        SetFalseAll();
        animator.SetTrigger("die");
        state = HState.Die;
    }

    public void PlayGethit()
    {
        SetFalseAll();
        animator.SetTrigger("gethit");
        state = HState.Gethit;
    }

    public void PlayIntonate()
    {
        SetFalseAll();
        animator.SetBool("intonate", true);
        state = HState.Intonate;
    }

    public void AttackEnd()
    {
        state = HState.None;
        PlayIdle();
    }

    public void GethitEnd()
    {
        state = HState.None;
        PlayIdle();
    }

    public void PlaySwitchover()
    {
        state = HState.switchover;
        animator.SetTrigger("switchover");
    }

    public void SwitchoverEnd()
    {
        state = HState.None;
        PlayIdle();
    }

    // ========== 新增：二段攻击/技能/跳跃 ==========
    // 二段攻击动画
    public void PlayAttack2()
    {
        SetFalseAll();
        animator.SetTrigger("attack2");
        state = HState.Attack2;
    }

    // 技能动画
    public void PlaySkill()
    {
        if (state == HState.Attack || state == HState.Attack2 || state == HState.Jump)
            return; // 攻击/跳跃时无法释放技能
        SetFalseAll();
        animator.SetTrigger("skill");
        state = HState.Skill;
    }

    // 跳跃动画
    public void PlayJump()
    {
        if (state == HState.Attack || state == HState.Attack2 || state == HState.Skill)
            return; // 攻击/技能时无法跳跃
        SetFalseAll();
        animator.SetTrigger("jump");
        animator.SetBool("Grounded",false);
        state = HState.Jump;
    }

    // 动画结束回调（需在Animator的动画事件中绑定）
    public void Attack2End()
    {
        state = HState.None;
        PlayIdle();
    }

    public void SkillEnd()
    {
        state = HState.None;
        PlayIdle();
    }
    public void EnterWaKuang()
    {
        SetFalseAll();
        state = HState.WaKuang;
        animator.SetBool("wakuang",true);
    }
    public void JumpEnd()
    {
        state = HState.None;
        PlayIdle();
    }

    internal void ExitWaKuang()
    {
        SetFalseAll();
        state = HState.Idle;
        animator.SetBool("wakuang", false );
    }

    internal void EnterKanShu()
    {
        SetFalseAll();
        state = HState.KanSHu;
        animator.SetBool("kanshu", true);
    }

    internal void ExitKanShu()
    {
        SetFalseAll();
        state = HState.Idle;
        animator.SetBool("kanshu", false);
    }

    internal void SetGround(bool v)
    {
        animator.SetBool("Grounded", v);
    }


    internal void PlayClimbIdle()
    {
        SetFalseAll();
        state = HState.Climb;
        animator.SetBool("Climb", true);
        animator.SetBool("walk", false );
    }
    internal void PlayClimbMove()
    {
        SetFalseAll();
        state = HState.Climb;
        animator.SetBool("walk", true);
    }
    internal void PlayClimbEnd()
    {
        SetFalseAll();
        state = HState.Idle;
        animator.SetBool("Climb", false );
        animator.SetBool("walk", false );
    }
}