using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Player : MonoBehaviour
{
    // 单例模式：通过静态Instance属性让其他脚本访问玩家对象
    public static Player Instance { get; private set; }

    // 定义当选中柜台发生变化时触发的事件

    public event EventHandler<OnseletedCounterChangedEventArgs> OnSelectedCounterChanged;

    // 事件参数类：传递当前选中的柜台
    public class OnseletedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter; // 当前选中的柜台对象
    }

    [SerializeField] private float moveSpeed = 7f;      // 玩家移动速度（单位/秒）
    [SerializeField] private GameInput gameInput;       // 输入系统引用（需在Inspector面板关联）
    [SerializeField] private LayerMask countersLayerMask; // 射线检测的层级过滤（勾选柜台所在层）

    private bool isWalking;            // 是否处于移动状态
    private Vector3 lastInteractDir;   // 最后记录的交互方向（用于射线检测）
    private ClearCounter selectedCounter; // 当前选中的柜台缓存

    private void Start()
    {
        // 订阅输入系统的交互事件（当按下E键时触发）
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    // 处理交互按键事件的回调方法
    // sender：事件发送者（GameInput对象）
    // e：事件参数（此处未使用）
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        // 调试输出当前选中的柜台名称（?? 是null合并运算符）
        Debug.Log($"尝试交互，当前选中：{selectedCounter?.gameObject.name ?? "null"}");

        if (selectedCounter != null)
        {
            // 调用选中柜台的交互方法
            selectedCounter.Interact();
        }
    }

    private void Awake()
    {
        // 单例模式初始化：确保场景中只有一个Player实例
        if (Instance != null)
            Debug.LogError("There is more than one Player Instance");
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();    // 每帧处理移动
        HandleInteractions();// 每帧处理交互检测
    }

    // 外部获取移动状态的方法（供动画脚本等调用）
    public bool IsWalking()
    {
        return isWalking;
    }

    // 处理与柜台的交互检测逻辑
    private void HandleInteractions()
    {
        // 获取输入方向（已标准化）
        Vector2 inputVector = gameInput.GetMovementVectorNormarlized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // 当有输入时更新最后交互方向
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f; // 射线检测距离

        // 执行射线检测参数说明：
        // - origin：射线起点（玩家当前位置）
        // - direction：射线方向（最后交互方向）
        // - hitInfo：输出参数，存储碰撞信息
        // - maxDistance：最大检测距离
        // - layerMask：层级过滤掩码
        if (Physics.Raycast(
            transform.position,
            lastInteractDir,
            out RaycastHit raycastHit,
            interactDistance,
            countersLayerMask))
        {
            // 尝试从碰撞物体获取ClearCounter组件
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // 如果当前选中柜台与检测到的不同则更新
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
                else
                {
                    // 相同柜台时也执行更新（可优化为不重复调用）
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                // 碰撞物体不是柜台时清空选中
                SetSelectedCounter(null);
            }
        }
        else
        {
            // 未检测到任何物体时清空选中
            SetSelectedCounter(null);
        }

        // 调试输出当前碰撞物体名称
        Debug.Log($"检测到物体：{raycastHit.transform?.gameObject.name ?? "null"}");
    }

    // 处理玩家移动逻辑
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormarlized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime; // 计算本帧应移动距离
        float playerRadius = .6f;    // 玩家胶囊碰撞体半径
        float playerHeight = 2f;     // 玩家胶囊碰撞体高度

        // 使用胶囊体检测前方障碍物参数说明：
        // - point1：胶囊体底部中心点
        // - point2：胶囊体顶部中心点（高度为playerHeight）
        // - radius：胶囊体半径
        // - direction：移动方向
        // - maxDistance：最大检测距离
        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance);

        // 处理斜向移动卡墙问题（分离X/Z轴检测）
        if (!canMove)
        {
            // 尝试仅沿X轴移动
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance);

            if (canMove)
            {
                moveDir = moveDirX; // 仅允许X轴移动
            }
            else
            {
                // 尝试仅沿Z轴移动
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ; // 仅允许Z轴移动
                }
                else
                {
                    // 两个方向都无法移动时不执行操作
                }
            }
        }

        // 执行移动
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // 根据移动方向判断是否处于移动状态
        isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 5f; // 旋转平滑过渡速度
        // 使用球面插值平滑旋转朝向
        transform.forward = Vector3.Slerp(
            transform.forward, // 当前朝向
            moveDir,           // 目标朝向
            rotationSpeed * Time.deltaTime); // 插值系数
    }

    // 更新选中柜台并触发事件
    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        // 更新当前选中柜台
        this.selectedCounter = selectedCounter;

        // 触发事件通知所有订阅者
        OnSelectedCounterChanged?.Invoke(
            this,
            new OnseletedCounterChangedEventArgs
            {
                selectedCounter = selectedCounter
            });
    }
}