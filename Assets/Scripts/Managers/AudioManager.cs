using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Transform sfxContainer;
    [SerializeField] private AudioSource sfxSource;

#region UNITY
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
#endregion

#region PUBLIC
    public void PlayAudio(AudioClip[] audioClips, Transform spawnPoint, float volume)
    {
        AudioSource source = Instantiate(sfxSource, spawnPoint.position, Quaternion.identity, sfxContainer);

        int index = Random.Range(0, audioClips.Length);
        source.clip = audioClips[index];
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, source.clip.length);
    }
#endregion
}
