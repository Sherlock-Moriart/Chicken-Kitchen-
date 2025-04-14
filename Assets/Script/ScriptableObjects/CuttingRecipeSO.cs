using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject  // 被切后的东西
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;//需要切（F）的次数

}
