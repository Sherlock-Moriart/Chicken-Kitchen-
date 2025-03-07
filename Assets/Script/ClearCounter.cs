using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObject; // 番茄预制体（需在Inspector设置）
    [SerializeField] private Transform CounterTopPoint; // 生成点（需指定柜台顶部子物体）

    public void Interact()
    {
        // 验证关键引用
        if (this.kitchenObject == null)
        {
            Debug.LogError("番茄预制体未设置！");
            return;
        }

        if (CounterTopPoint == null)
        {
            Debug.LogError("生成点未设置！");
            return;
        }

        // 实例化并设置位置（使用世界坐标）
        Transform kitchenObjectTransform = Instantiate(this.kitchenObject.prefab);
        kitchenObjectTransform.position = CounterTopPoint.position;

        // 可选：重置旋转
        kitchenObjectTransform.rotation = Quaternion.identity;

        Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
    }
}