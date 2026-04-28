using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuLongFSM
{
    public enum FSMState
    {
        None = 0,
        Idle,
        Run,
        Walk,
        Attack,
        Attack2, // 二段攻击
        Skill,   // 技能
        Jump,    // 跳跃
        Patrol,
        Hit,
        WaKuang,
        KanShu,
        Climb,
    }
    [Serializable]
    public class FSMManager<T>
    {
        public FSMState cutFSMState;
        private FSMIState<T> cutFSMIState;
        private Dictionary<FSMState, FSMIState<T>> fSMStateDic = new Dictionary<FSMState, FSMIState<T>>();
        private T fSMData;

        public FSMManager(T fSMData)
        {
            this.fSMData = fSMData;
        }
        public void Register(FSMState fSMState, FSMIState<T> fSMIState)
        {
            fSMIState.fSMData = fSMData;
            fSMIState.fSMManager = this;
            fSMStateDic.Add(fSMState, fSMIState);
        }
        // 修复原Unregister的BUG：应该移除而非添加
        public void Unregister(FSMState fSMState)
        {
            if (fSMStateDic.ContainsKey(fSMState))
                fSMStateDic.Remove(fSMState);
        }

        public void Switch(FSMState fSMState)
        {
            if (!fSMStateDic.ContainsKey(fSMState))
            {
                Debug.Log("当前状态未注册：" + fSMState);
                return;
            }
            if (cutFSMState == fSMState) return;

            FSMIState<T> fSMIState = fSMStateDic[fSMState];
            cutFSMIState?.OnExit();
            fSMIState.OnEnter();
            cutFSMIState = fSMIState;
            cutFSMState = fSMState;
        }
        public void OnUpdate()
        {
            cutFSMIState?.OnUpdate();
        }
    }
}