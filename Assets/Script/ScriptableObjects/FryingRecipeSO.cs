using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject  // 被油炸后的东西
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTimerMax;//需要炸的时间的次数
}
