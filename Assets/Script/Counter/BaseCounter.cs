// 这个类是所有厨房柜台的基类，处理物品放置和基础交互逻辑
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    // 物品生成位置锚点（在Unity编辑器中拖拽柜台上方的空物体到这里）
    [SerializeField] private Transform counterTopPoint;

    // === 运行时状态 ===
    private KitchenObject kitchenObject; // 当前柜台持有的物品对象（比如食材/盘子）

    // 交互方法（需要子类具体实现）
    public virtual void Interact(Player player)
    {
        // virtual表示子类必须/可以重写这个方法
        // 参数player表示与之交互的玩家对象
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("F键柜台交互");
    }

    // === 实现IKitchenObjectParent接口 ===

    // 获取物品应该跟随的位置（柜台上方的指定位置）
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    // 设置当前持有的物品
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    // 获取当前持有的物品
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // 清空当前持有的物品
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // 检查是否有物品
    public bool HasKitchenObject()
    {
        return (kitchenObject != null);
    }
}