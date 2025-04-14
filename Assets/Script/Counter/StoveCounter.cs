using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter; // 引入CuttingCounter中的静态成员
using System;

// 炉灶柜台类，继承自BaseCounter
public class StoveCounter : BaseCounter,IHasProgress
{
    // 定义了一个事件，用于进度条变化的信息
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    // 定义状态变化事件，当炉灶状态改变时触发
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    // 事件参数类，用于传递状态变化信息
    public class OnStateChangedEventArgs : EventArgs
    {
        // 当前状态
        public State state;
    }

    // 炉灶状态枚举
    public enum State
    {
        Idle,    // 空闲状态
        Frying,  // 炸制状态
        Fried,   // 已炸好状态
        Burned   // 烧焦状态
    }

    // 炸制配方数组（通过Inspector面板赋值）
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    // 烧焦配方数组（通过Inspector面板赋值）
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    // 当前状态
    private State state;
    // 炸制计时器
    private float fryingTimer;
    // 烧焦计时器
    private float burningTimer;
    // 当前炸制配方
    private FryingRecipeSO fryingRecipeSO;
    // 当前烧焦配方
    private BurningRecipeSO burningRecipeSO;

    // 在游戏开始时初始化状态
    private void Start()
    {
        state = State.Idle; // 初始状态为空闲
    }

    // 每帧更新逻辑
    private void Update()
    {
        // 如果当前柜台有物品
        if (HasKitchenObject())
        {
            // 根据当前状态执行不同逻辑
            switch (state)
            {
                case State.Idle:
                    // 空闲状态无需处理
                    break;

                case State.Frying:
                    // 累加炸制时间
                    fryingTimer += Time.deltaTime;//累计帧间隔，Time.time是游戏运行的总时间，会导致计时器瞬间达到最大值

                    // 调用进度变化事件，通知订阅者当前进度
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {    // 计算并设置归一化进度值（0到1之间）
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    // 如果炸制时间超过最大值
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // 销毁当前物品
                        GetKitchenObject().DestroySelf();
                        // 生成炸好的物品
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        Debug.Log("炸好了"); // 打印日志
                        // 更新状态为已炸好
                        state = State.Fried;
                        // 重置烧焦计时器
                        burningTimer = 0f;
                        // 获取烧焦配方
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        // 触发状态变化事件
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;

                case State.Fried:
                    // 累加烧焦时间
                    burningTimer += Time.deltaTime;

                    // 调用进度变化事件，通知订阅者当前进度
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {    // 计算并设置归一化进度值（0到1之间）
                        progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
                    });

                    // 如果烧焦时间超过最大值
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // 销毁当前物品
                        GetKitchenObject().DestroySelf();
                        // 生成烧焦的物品
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        Debug.Log("炸焦了"); // 打印日志
                        // 更新状态为烧焦
                        state = State.Burned;

                        // 触发状态变化事件
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        // 调用进度变化事件，通知订阅者当前进度
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {    // 计算并设置归一化进度值（0到1之间）
                            progressNormalized = 0f//炸焦了就关闭进度条 
                        });
                    }
                    break;

                case State.Burned:
                    // 烧焦状态无需处理
                    break;
            }

            // 打印当前状态（调试用）
            Debug.Log(state);
        }
    }

    // 玩家交互逻辑
    public override void Interact(Player player)
    {
        // 情形1：当前柜台为空
        if (!HasKitchenObject())
        {
            // 检查玩家是否携带有效可加工物品
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // 将玩家携带的物品转移到柜台
                player.GetKitchenObject().SetKitchenObjectParent(this);
                // 获取对应的炸制配方
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                // 更新状态为炸制中
                state = State.Frying;
                // 重置炸制计时器
                fryingTimer = 0f;

                // 触发状态变化事件
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                // 调用进度变化事件，通知订阅者当前进度
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {    // 计算并设置归一化进度值（0到1之间）
                    progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                });
            }
            else
            {
                // 如果玩家携带的物品无法加工，不做处理
            }
        }
        // 情形2：当前柜台有物品
        else
        {
            if (!player.HasKitchenObject())  // 玩家未持有物品时执行
            {
                // 将柜台中的物品转移到玩家
                GetKitchenObject().SetKitchenObjectParent(player);
                // 更新状态为空闲
                state = State.Idle;

                // 触发状态变化事件
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                // 调用进度变化事件，通知订阅者当前进度
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {    // 计算并设置归一化进度值（0到1之间）
                    progressNormalized = 0f//关闭进度条 
                });
            }
            else
            {
                // 如果玩家已持有物品，不做处理
            }
        }
    }

    // 检查是否存在匹配的炸制配方
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    /// <summary>
    /// 通过原材料获取对应成品（配方匹配核心方法）
    /// </summary>
    /// <param name="inputKitchenObjectSO">输入物品数据</param>
    /// <returns>成品物品数据/null（无配方时）</returns>
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null ? fryingRecipeSO.output : null;
    }

    /// <summary>
    /// 线性搜索匹配炸制配方
    /// 时间复杂度：O(n)
    /// 优化建议：运行时转换为字典提升查询效率
    /// </summary>
    /// <param name="inputKitchenObjectSO">查找的原材料</param>
    /// <returns>匹配的配方/null</returns>
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        // 遍历所有炸制配方
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;  // 返回首个匹配项
            }
        }
        return null;  // 未找到匹配项时返回null
    }

    // 线性搜索匹配烧焦配方
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        // 遍历所有烧焦配方
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO) 
            {
                return burningRecipeSO;  // 返回首个匹配项
            }
        }
        return null;  // 未找到匹配项时返回null
    }
}