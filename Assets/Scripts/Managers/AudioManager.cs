using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [field: SerializeField]
    public SoundLibrary SoundLibrary { get; private set; }

    [SerializeField] private Transform soundContainer;
    [SerializeField] private AudioSource audioSource;

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

    void Start()
    {
        // Play background music
        PlayAudio(SoundLibrary.bgms, transform, 1f, true);
    }
#endregion

#region PUBLIC
    public void PlayAudio(AudioClip[] audioClips, Transform spawnPoint, float volume, bool looping)
    {
        AudioSource source = Instantiate(audioSource, spawnPoint.position, Quaternion.identity, soundContainer);

        int index = Random.Range(0, audioClips.Length);
        source.clip = audioClips[index];
        source.volume = volume;
        source.Play();

        if (!looping) 
            Destroy(source.gameObject, source.clip.length);
        else 
            source.loop = true;
    }
#endregion
}
