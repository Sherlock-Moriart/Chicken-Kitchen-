using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent//接口
{
    Transform GetKitchenObjectFollowTransform(); // 获取物品挂载点（柜台/玩家手持点）
    void SetKitchenObject(KitchenObject kitchenObject); // 设置当前持有的物品
    KitchenObject GetKitchenObject(); // 获取当前持有的物品
    void ClearKitchenObject(); // 清空持有的物品
    bool HasKitchenObject(); // 是否持有物品
}

