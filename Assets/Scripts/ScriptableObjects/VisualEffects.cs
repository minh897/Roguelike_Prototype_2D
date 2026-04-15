using System;
using UnityEngine;

[Serializable]
public struct FloatingTextData
{
    public FloatingText prefab;
    public float animSpeed;
    public float expireTime;
}

[CreateAssetMenu(fileName = "VisualEffects", menuName = "Scriptable Objects/VisualEffects")]
public class VisualEffects : ScriptableObject
{
    public FloatingTextData floatingText;
}
