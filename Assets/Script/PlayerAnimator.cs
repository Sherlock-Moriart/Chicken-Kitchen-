using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";  // 动画参数名（避免硬编码）

    [SerializeField] private Player player;         // 引用 Player 脚本
    private Animator animator;                      // 角色 Animator 组件

    private void Awake()
    {
        // 获取当前对象的 Animator 组件
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 根据玩家是否在行走，更新动画状态
        animator.SetBool(IS_WALKING, player.IsWalking());

        /*第一个参数 name：动画参数的名称（字符串类型）。
        第二个参数 value：要为该参数设置的布尔值 true或false*/
    }
}