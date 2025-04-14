using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 炉灶视觉控制器
// 功能：根据灶台状态控制炉火与粒子效果的显示/隐藏
public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;          // 关联的灶台逻辑控制器
    [SerializeField] private GameObject stoveOnGameObject;       // 炉火显示对象（火焰模型）
    [SerializeField] private GameObject particlesGameObject;     // 油炸粒子效果（油锅气泡）

    // 初始化：注册状态变化监听
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    /// <summary>
    /// 灶台状态变化事件处理
    /// 逻辑：当处于炸制阶段（Frying/Fried）时显示视觉效果
    /// </summary>
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // 双视觉效果统一控制：炉火+粒子同时切换
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveOnGameObject.SetActive(showVisual);     // 控制火焰显示
        particlesGameObject.SetActive(showVisual);   // 控制油锅气泡
    }
}
