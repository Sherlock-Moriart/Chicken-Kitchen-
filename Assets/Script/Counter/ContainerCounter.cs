using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO; // 要生成的厨房物品配置（需在Inspector设置）
    // === 运行时状态 ===

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            // 触发事件（通知所有订阅者物品已经被拿取）
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

        }



        //if (this.kitchenObjectSO == null)
        //{
        //    Debug.LogError("[配置错误] 需要指定KitchenObjectSO！");
        //    return;
        //}
        //if (counterTopPoint == null)
        //{
        //    Debug.LogError("[场景配置错误] 缺失counterTopPoint位置锚点！");
        //    return;
        //}
        // ===状态判断逻辑 ===
    }
}



