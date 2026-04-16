using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [field: SerializeField]
    public VisualEffects VFXS { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TriggerFloatingText(string text, Vector3 spawnPoint)
    {
        var prefab = VFXS.floatingText.prefab;
        float animSpeed = VFXS.floatingText.animSpeed;
        float expireTime = VFXS.floatingText.expireTime;

        var spawnEffect = Instantiate(prefab);
        spawnEffect.Init(text, animSpeed, expireTime, spawnPoint);
    }
}
