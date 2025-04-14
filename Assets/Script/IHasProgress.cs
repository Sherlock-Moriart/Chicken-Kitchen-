using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 进度追踪接口
/// 核心功能：为需要显示进度条的交互对象提供统一事件机制
/// 适用场景：烹饪过程、生产制作等需要进度反馈的交互行为
/// </summary>
public interface IHasProgress
{
    /// <summary>
    /// 进度变化事件
    /// 触发时机：当对象的进度状态发生变化时（如烹饪计时/制作进度更新）
    /// 监听实例：UI进度条、音效系统、视觉特效等
    /// </summary>
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    /// <summary>
    /// 进度事件参数类
    /// 封装标准化的进度数据（0到1范围）
    /// </summary>
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized; // 归一化进度值（0=未开始，1=完成，0.5=50%进度）
    }
}
