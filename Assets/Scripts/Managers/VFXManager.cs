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

    public void TriggerFloatingText(string text, Transform spawnPoint)
    {
        var prefab = VFXS.floatingText.prefab;
        float animSpeed = VFXS.floatingText.animSpeed;
        float expireTime = VFXS.floatingText.expireTime;

        var spawnEffect = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        spawnEffect.Init(text, animSpeed, expireTime);
    }
}
