using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject  //油炸好的东西
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;//炸焦的时间限制
}