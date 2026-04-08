using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Material originalMat;
    private SpriteRenderer sr;
    private Coroutine coroutine;

    [SerializeField] private Material replaceMat;
    [SerializeField] private float flashDuration = .15f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void PlayDamageFlash()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        sr.material = replaceMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originalMat;
    }
}
