using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 厨房物品交互系统核心组件
public class KitchenObject : MonoBehaviour
{
    // === 序列化字段 ===
    [SerializeField]
    private KitchenObjectSO kitchenObjectSO; // 关联的厨房物品数据对象（ScriptableObject）
                                             // 包含物品名称、图标、预制体等基本信息

    // === 私有字段 === 
    private IKitchenObjectParent kitchenObjectParent;      // 当前所在的柜台引用
                                            // 用于追踪物品所在位置逻辑

    // === 公共方法 ===

    /// <summary>
    /// 获取物品的配置数据
    /// </summary>
    /// <returns>关联的ScriptableObject数据资产</returns>
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    /// <summary>
    /// 设置物品当前所在的柜台
    /// （物品位置逻辑控制器）
    /// </summary>
    /// <param name="kitchenObjectParent">目标柜台实例</param>
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();//这里是先清理这个原先柜台上得东西再在下面把东西设置在后面得橱柜上
        }

        // 更新物品所在柜台的引用记录,设置新父对象
        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("橱柜上已经有东西了");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 获取当前物品所在的柜台
    /// </summary>
    /// <returns>如果未放置在柜台则返回null</returns>
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }


    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();

         Destroy(gameObject);
    }

    /// <summary>
    /// 生成厨房对象并将其绑定到指定父级对象
    /// </summary>
    /// <param name="kitchenObjectSO">厨房对象数据资产（包含预制体引用）</param>
    /// <param name="kitchenObjectParent">目标父级对象（需实现交互接口）</param>
    /// <returns>已生成的厨房对象实例</returns>
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        // 从ScriptableObject加载预制体并进行实例化
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        // 从实例化的游戏物体获取厨房对象逻辑组件
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        // 通过接口将对象绑定到父级容器（玩家/柜台等）
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        // 返回生成的对象引用以便后续操作（烹饪/组合等）
        return kitchenObject;
    }


}
