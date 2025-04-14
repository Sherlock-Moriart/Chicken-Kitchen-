// 这个脚本用于控制容器柜台的动画效果
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisua : MonoBehaviour
{
    // 序列化字段
    [SerializeField] private CuttingCounter cuttingCounter; // 对应的容器柜台

    private Animator animator; // 控制动画的组件

    // 常量：动画触发器参数的名称（避免拼写错误）
    private const string CUT = "Cut";

    // 初始化时自动调用（比Start更早）
    private void Awake()
    {
        // 获取挂载在同一个游戏对象上的Animator组件
        animator = GetComponent<Animator>();
    }

    // 游戏开始时调用
    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_Oncut;
        // 订阅容器柜台的事件：当玩家拿取物品时触发

    }

    private void CuttingCounter_Oncut(object sender,System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }


}