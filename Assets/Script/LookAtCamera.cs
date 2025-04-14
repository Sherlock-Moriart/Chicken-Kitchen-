using UnityEngine;

/// <summary>
/// 3D物体朝向相机控制器
/// 功能：控制物体以不同模式始终面向摄像机
/// 执行阶段：LateUpdate保证在摄像机移动后更新朝向
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    /// <summary>
    /// 朝向模式枚举
    /// </summary>
    private enum Mode
    {
        LookAt,             // 直接注视摄像机
        LookAtInverted,      // 反向注视（背对摄像机）
        CameraForward,       // 与摄像机同方向对齐
        CameraForwardInverted // 与摄像机反方向对齐
    }

    [Tooltip("选择物体朝向的计算模式")]
    [SerializeField] private Mode mode; // 在Inspector面板选择模式

    /// <summary>
    /// 每帧最后执行的更新（保证摄像机移动后更新）
    /// </summary>
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                // 基础注视模式：物体正面始终对准摄像机
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                // 反向注视模式：物体背面始终对准摄像机
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera); // 看向与摄像机相反方向
                break;
            case Mode.CameraForward:
                // 方向同步模式：物体前向与摄像机前向完全一致
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                // 反向同步模式：物体前向与摄像机前向相反
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
} 