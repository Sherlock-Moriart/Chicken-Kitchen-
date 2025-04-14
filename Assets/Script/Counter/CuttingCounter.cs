using System;
using System.Collections;      // Unity基础协程支持
using System.Collections.Generic;  // 泛型数据结构支持
using UnityEngine;
using UnityEngine.EventSystems;             // Unity引擎核心功能

/// <summary>
/// 切割操作台核心逻辑
/// 实现功能：
/// 1. 接收玩家放置的原材料
/// 2. 进行多次切割转换为成品
/// 3. 允许取回成品
/// 继承自基础交互柜台基类
/// </summary>
public class CuttingCounter : BaseCounter,IHasProgress
{
    // 序列化字段区域-------------------------------------

    [Header("切割配方配置")]
    [Tooltip("所有可处理的切割配方数据")]
    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;  // 切割配方数据库（原材料->成品映射关系）

    // 状态记录区域--------------------------------------

    private int cuttingProgress; // 当前切割进度计数（需要达到配方要求次数才能转换）

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;// 定义了一个事件，用于进度条变化的信息
 
    public event EventHandler OnCut;
    public override void Interact(Player player)
    {
        // 情形1：当前柜台为空
        if (!HasKitchenObject())
        {
            // 检查玩家是否携带有效可加工物品
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);  // 完成物品所有权转移
                cuttingProgress = 0;  // 初始化切割进度计数器


                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized =(float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
        }
        // 情形2：当前柜台有物品
        else
        {
            if (!player.HasKitchenObject())  // 玩家未持有物品时执行
            {
                GetKitchenObject().SetKitchenObjectParent(player);  // 物品转移给玩家
            }
            else
            {
                // TODO: 后续可扩展合成系统（如汉堡组装逻辑）
            }
        }
    }

    /// <summary>
    /// 副交互逻辑（F键切割）
    /// 执行切割操作：
    /// 1. 每次点击累积切割进度
    /// 2. 满足进度要求后触发物品转换
    /// </summary>
    public override void InteractAlternate(Player player)
    {
        // 有效性验证：柜台有物品且可被切割
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;  // 递增当前切割次数


            OnCut?.Invoke(this, EventArgs.Empty);
            // 获取当前物品对应的切割配方
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());


            // 调用进度变化事件，通知订阅者当前进度
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                // 计算并设置归一化进度值（0到1之间）
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });

            // 切割进度达标检测
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)  // 达成所需次数
            {
                // 获取并生成目标成品
                KitchenObjectSO outputObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();  // 移除原材料
                KitchenObject.SpawnKitchenObject(outputObjectSO, this);  // 生成成品到当前柜台
            }
        }
    }

    // 配方系统辅助方法区---------------------------------

    /// <summary>
    /// 验证输入物品是否为有效原材料
    /// </summary>
    /// <param name="inputKitchenObjectSO">待验证物品数据</param>
    /// <returns>是否存在对应配方</returns>
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    /// <summary>
    /// 通过原材料获取对应成品（配方匹配核心方法）
    /// </summary>
    /// <param name="inputKitchenObjectSO">输入物品数据</param>
    /// <returns>成品物品数据/null（无配方时）</returns>
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO recipe = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return recipe != null ? recipe.output : null;
    }

    /// <summary>
    /// 线性搜索匹配切割配方
    /// 时间复杂度：O(n)
    /// 优化建议：运行时转换为字典提升查询效率
    /// </summary>
    /// <param name="inputKitchenObjectSO">查找的原材料</param>
    /// <returns>匹配的配方/null</returns>
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;  // 首个匹配项立即返回
            }
        }
        return null;  // 遍历未找到匹配项
    }
}
