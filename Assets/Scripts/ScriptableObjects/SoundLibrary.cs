using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Scriptable Objects/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    public AudioClip[] victory;
    public AudioClip[] bgms;
    public AudioClip[] sfxFootSteps;
    public AudioClip[] sfxAttacks;
    public AudioClip[] sfxEatFoods;
    public AudioClip[] sfxPlayerDowns;
}
