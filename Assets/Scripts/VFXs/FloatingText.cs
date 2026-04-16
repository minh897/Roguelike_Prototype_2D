using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private Animator _anim;
    
    private float _expireTime;

    void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshPro>();
        _anim = GetComponentInChildren<Animator>();
    }

    public void Init(string text, float speed, float expireTime, Vector3 spawnPoint)
    {
        gameObject.SetActive(false);

        _textMesh.text = text;
        _anim.speed = speed;
        _expireTime = expireTime;
        transform.position = spawnPoint;

        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(gameObject, _expireTime);
    }
}
