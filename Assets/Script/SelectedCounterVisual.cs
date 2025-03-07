using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter; // 需要显示高光的柜台对象（需在Inspector关联）
    [SerializeField] private GameObject visualGameobject; // 高光显示用的子物体（需在Inspector关联）

    private void Start()
    {
        // 订阅玩家选中柜台变化事件
        Player.Instance.OnSelectedCounterChanged += Player_OnseletedCounterChanged;
    }

    // 当选中柜台变化时的回调方法
    // sender：事件发送者（Player对象）
    // e：事件参数（包含当前选中的柜台）
    private void Player_OnseletedCounterChanged(object sender, Player.OnseletedCounterChangedEventArgs e)
    {
        // 如果当前选中的柜台是本脚本关联的柜台
        if (e.selectedCounter == clearCounter)
        {
            Show(); // 显示高光
        }
        else
        {
            Hide(); // 隐藏高光
        }
    }

    // 激活高光显示
    private void Show()
    {
        visualGameobject.SetActive(true);
    }

    // 关闭高光显示
    private void Hide()
    {
        visualGameobject.SetActive(false);
    }
}