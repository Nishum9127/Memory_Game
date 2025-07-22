using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;
    public AudioClip GridBtnClick;
    public AudioClip btnClick;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;

        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }

    public void PlayFlipSound()
    {
        audioSource.PlayOneShot(flipClip);
    }

    public void PlayMatchSound()
    {
        audioSource.PlayOneShot(matchClip);
    }

    public void PlayMismatchSound()
    {
        audioSource.PlayOneShot(mismatchClip);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverClip);
    }
    public void PlayGridCreationSound()
    {
        audioSource.PlayOneShot(GridBtnClick);
    }
    public void PlayBtnClickSound()
    {
        audioSource.PlayOneShot(btnClick);
    }

    public void PlayBackGroundSound()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopBackGroundSound()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

}
