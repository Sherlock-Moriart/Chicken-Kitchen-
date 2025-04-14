
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 进度条显示控制器（需挂载至含Image组件的UI对象）
public class ProgressBarUI : MonoBehaviour
{
    // 可视化配置区域
    //[Header("关键组件关联配置")]
    //[Tooltip("拖拽你要监控的切菜台对象")]


    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage; // 必须设置为Filled类型才有效果
    private IHasProgress hasProgress; 

    /**************************************************
     * 初始化阶段
     **************************************************/
    private void Start()
    {
        hasProgress= hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null)
        {
            Debug.LogError("进度条UI错误");
        }
        // 订阅切菜台进度变化事件
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }


    // 当切菜进度变化时通过事件触发
    private void HasProgress_OnProgressChanged(object sender,IHasProgress.OnProgressChangedEventArgs e)
    {
        // 核心逻辑：将进度值（0-1）映射到UI填充比例
        // 工作原理：Image设置为Filled模式时，fillAmount控制显示比例
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized <= 0f || e.progressNormalized >= 1f)
        {
            Hide();
        }
        else
        {
             Show();
        }


    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
   
}

/********************* 关键注释说明 ********************
▌参数说明：
 - progressNormalized：规范化的进度值（范围0.0~1.0）
   0=无进度，0.5=50%完成，1=进度完成

▌实现说明：
1. UI可视化设置：
   - 需将barImage的Image Type设为Filled
   - 可选择Horizontal、Radial等填充模式

3. 常见问题：
   - 如果进度条无变化，检查：
     a) barImage是否设置正确的Fill类型
     b) 切菜台是否通过事件传递正确参数
*/

