using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    // 定义交互事件（当按下E键时触发）
    public event EventHandler OnInteractAction;

    // Unity新输入系统的输入动作资源引用
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        // 初始化输入系统
        playerInputActions = new PlayerInputActions();
        // 启用Player输入映射（对应Input Actions中的Action Maps）
        playerInputActions.Player.Enable();

        // 绑定交互动作（E键）的回调方法
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    // 交互动作触发时的回调方法
    // context：输入系统提供的回调参数（包含输入详细信息）
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // 触发所有订阅了OnInteractAction的事件监听器
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // 获取标准化后的移动输入向量（范围[-1,1]）
    public Vector2 GetMovementVectorNormarlized()
    {
        // 从输入系统读取原始输入向量（可能未标准化）
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // 标准化处理（使向量长度为1，保持方向不变）
        inputVector = inputVector.normalized;

        return inputVector;
    }
}