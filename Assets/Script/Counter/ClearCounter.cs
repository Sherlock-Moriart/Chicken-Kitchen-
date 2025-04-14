using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 可交互的清除柜台系统
/// 处理物体的放置/取出逻辑
/// </summary>
public class ClearCounter : BaseCounter//只能有一个基类，但是可以扩展多个接口。ClearCounter意思是普通没有任何东西的柜子
{
    // === 配置参数 ===
    [SerializeField] private KitchenObjectSO kitchenObjectSO; // 要生成的厨房物品配置（需在Inspector设置）

    //public override void Interact(Player player)
    //{

    //    //if (this.kitchenObjectSO == null)
    //    //{
    //    //    Debug.LogError("[配置错误] 需要指定KitchenObjectSO！");
    //    //    return;
    //    //}
    //    //if (counterTopPoint == null)
    //    //{
    //    //    Debug.LogError("[场景配置错误] 缺失counterTopPoint位置锚点！");
    //    //    return;
    //    //}


    //    // 检查当前柜台是否没有存放厨房物品
    //    if (!HasKitchenObject())   // this.HasKitchenObject() 的简写，判断当前柜台空置
    //    {
    //        // 如果玩家手中持有物品（调用 Player 类的 HasKitchenObject() 方法）
    //        if (player.HasKitchenObject())
    //        {
    //            /* 将玩家持有的物品转移到此柜台 
    //             * 流程：
    //             * 1. 获取玩家手中的 KitchenObject 组件：GetKitchenObject()
    //             * 2. 调用 SetKitchenObjectParent(this)：
    //             *    - 解除与原持有者（玩家）的关联
    //             *    - 设置新持有者为当前柜台
    //             *    - 更新物品的 Transform（定位到柜台顶部） */
    //            player.GetKitchenObject().SetKitchenObjectParent(this);
    //        } 
    //        else{
    //            // 柜台有物品：若玩家未持有物品，则取走物品
    //            if (!player.HasKitchenObject())
    //            {
    //                GetKitchenObject().SetKitchenObjectParent(player);
    //            }
    //            else
    //            {

    //            }
    //        }
    //    }


    //}


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 柜台空置：玩家放置物品
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // 柜台有物品：玩家取走物品
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player); // ✅ 关键修复点
            }
            else
            {
                // 玩家手中有物品时的扩展逻辑（例如合成）
            }
        }
    }
}
