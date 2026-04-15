using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private Animator _anim;
    
    private float _expireTime;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _anim = GetComponent<Animator>();
    }

    public void Init(string text, float speed, float expireTime)
    {
        gameObject.SetActive(false);

        _textMesh.text = text;
        _anim.speed = speed;
        _expireTime = expireTime;

        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(gameObject, _expireTime);
    }
}
