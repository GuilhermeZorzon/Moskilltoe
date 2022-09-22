using Unity.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedSound", menuName = "Scripted Sounds/New ScriptedSound")]
public class ScriptableSound : ScriptableObject
{
    public AudioClip clip;
    public float volume = 1;
    public float pitch = 1;

    private AudioSource audioSource = null;
    
    public AudioSource Play()
    {
        if (this.audioSource == null)
        {
            var audioSourceObj = new GameObject("AudioSource", typeof(AudioSource));
            this.audioSource = audioSourceObj.GetComponent<AudioSource>();
        }

        this.audioSource.clip = this.clip;
        this.audioSource.volume = this.volume;
        this.audioSource.pitch = this.pitch;
        this.audioSource.Play();

        return audioSource;
    }

    public void Stop()
    {
        this.audioSource.Stop();
    }
}
