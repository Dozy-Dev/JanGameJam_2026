using UnityEngine;

public class UnitAudioHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepSounds; // Array to hold different footstep clips
    public AudioClip[] lightAttackSounds; // Array to hold different clips
    public AudioClip[] heavyAttackSounds; // Array to hold different clips
    public AudioClip deathSound;
    public AudioClip[] hitSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Ensure an AudioSource is present
        if (audioSource == null)
        {
            Debug.LogError("UnitAudioHandler needs an AudioSource component!");
        }
    }

    // This function will be called by the Animation Event
    public void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0 && audioSource != null)
        {
            // Choose a random sound from the array
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            // Play the clip as a one-shot so it doesn't interrupt existing sounds
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayAttackingSound()
    {
        if (lightAttackSounds.Length > 0 && audioSource != null)
        {
            // Choose a random sound from the array
            AudioClip clip = lightAttackSounds[Random.Range(0, lightAttackSounds.Length)];
            // Play the clip as a one-shot so it doesn't interrupt existing sounds
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayHeavyAttackingSound()
    {
        if (heavyAttackSounds.Length > 0 && audioSource != null)
        {
            // Choose a random sound from the array
            AudioClip clip = heavyAttackSounds[Random.Range(0, heavyAttackSounds.Length)];
            // Play the clip as a one-shot so it doesn't interrupt existing sounds
            audioSource.PlayOneShot(clip);
        }
    }
    public void PlayHitSound()
    {
        if (hitSounds.Length > 0 && audioSource != null)
        {
            // Choose a random sound from the array
            AudioClip clip = hitSounds[Random.Range(0, hitSounds.Length)];
            // Play the clip as a one-shot so it doesn't interrupt existing sounds
            audioSource.PlayOneShot(clip);
        }
    } 
    
    public void PlayDeathSound()
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
