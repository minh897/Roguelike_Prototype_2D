using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    [SerializeField] private FloatingText textPrefab;
    [SerializeField] private float animSpeed = 1f;
    [SerializeField] private float expireTime = 1f;

    // public void TriggerEffect(string text, Transform spawnPoint)
    // {
    //     // Instantiate the effect at the GameObject position
    //     var newFloatingText = Instantiate(textPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
    //     newFloatingText.Init(text, animSpeed, expireTime);
    // }
}
