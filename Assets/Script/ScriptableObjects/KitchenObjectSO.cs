using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 创建资源菜单特性：允许在Unity编辑器中的Assets/Create菜单下生成该类型的ScriptableObject
// 菜单项默认名称为类名"KitchenObjectSO"
[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject // 继承自ScriptableObject，用于创建可配置的数据资源
{
    // 预制体字段：存储厨房物品的3D模型预制体（需在Inspector面板关联）
    public Transform prefab;

    // 图标字段：用于UI显示的2D图标（需在Inspector面板设置）
    public Sprite sprite;

    // 物品名称字段：该厨房物品的显示名称（例如"番茄"、"刀具"等）
    public string objectName;
}