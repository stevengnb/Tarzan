using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerGame : MonoBehaviour
{
    public static AudioManagerGame instance { get; private set; }
    [SerializeField] private List<AudioClip> footStep;
    [SerializeField] private List<AudioClip> footStepStone;
    [SerializeField] private List<AudioClip> footStepWood;
    [SerializeField] private List<AudioClip> jumpStep;
    [SerializeField] private List<AudioClip> other;
    [SerializeField] private List<AudioClip> bgm;
    private AudioSource footAudio;
    private AudioSource otherAudio;
    public AudioSource bgmAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            bgmAudio = gameObject.GetComponent<AudioSource>();
            footAudio = gameObject.AddComponent<AudioSource>();
            otherAudio = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FootStep()
    {
        int index = Random.Range(0, footStep.Count - 1);
        footAudio.clip = footStep[index];
        footAudio.volume = 0.3f;
        footAudio.pitch = 0.7f;
        footAudio.spatialBlend = 0.5f;
        footAudio.Play();
    }

    public void FootStepStone()
    {
        int index = Random.Range(0, footStepStone.Count - 1);
        footAudio.clip = footStepStone[index];
        footAudio.volume = 0.9f;
        footAudio.pitch = 0.7f;
        footAudio.spatialBlend = 0.5f;
        footAudio.Play();
    }

    public void FootStepWood()
    {
        int index = Random.Range(0, footStepWood.Count - 1);
        footAudio.clip = footStepWood[index];
        footAudio.volume = 1.5f;
        footAudio.pitch = 0.7f;
        //footAudio.spatialBlend = 0.5f;
        footAudio.Play();
    }

    public void Jump()
    {
        int index = Random.Range(0, 1);
        footAudio.clip = jumpStep[index];
        footAudio.volume = 0.5f;
        footAudio.pitch = 0.7f;
        footAudio.spatialBlend = 0.5f;
        footAudio.Play();
    }

    public void JumpStone()
    {
        int index = Random.Range(2, 3);
        footAudio.clip = jumpStep[index];
        footAudio.volume = 0.5f;
        footAudio.pitch = 0.7f;
        footAudio.spatialBlend = 0.5f;
        footAudio.Play();
    }

    public void JumpWood()
    {
        int index = Random.Range(4, 5);
        footAudio.clip = jumpStep[index];
        footAudio.volume = 1.5f;
        footAudio.pitch = 0.7f;
        footAudio.spatialBlend = 0.5f;
        footAudio.Play();
    }

    public void Talking(int text)
    {
        otherAudio.clip = other[text];
        otherAudio.volume = 0.85f;
        otherAudio.Play();
    }

    public void StopTalking()
    {
        otherAudio.Stop();
    }

    public void GrapplingSfx()
    {
        otherAudio.clip = other[2];
        otherAudio.volume = 0.4f;
        otherAudio.Play();
    }

    public void StopGrapplingSfx()
    {
        otherAudio.volume = 0.2f;
        StartCoroutine(GrapplingSmall(0.5f));
    }

    public void ButtonClicked()
    {
        otherAudio.clip = other[3];
        otherAudio.Play();
    }

    public void GameOverSound()
    {
        otherAudio.clip = other[4];
        otherAudio.volume = 1.5f;
        otherAudio.Play();
        StartCoroutine(GameOverTwo(2f));
    }

    public void PunchSound()
    {
        otherAudio.clip = other[6];
        otherAudio.volume = 0.2f;
        otherAudio.Play();
    }

    public void RockThrownSound()
    {
        otherAudio.clip = other[7];
        otherAudio.volume = 0.5f;
        otherAudio.Play();
    }

    public void ChangeCave()
    {
        bgmAudio.volume = 0.15f;
        StartCoroutine(SwitchBgm(0.25f, 1));
    }

    public void ChangeNormal()
    {
        bgmAudio.volume = 0.15f;
        StartCoroutine(SwitchBgm(0.25f, 0));
    }

    private IEnumerator SwitchBgm(float duration, int index)
    {
        yield return new WaitForSeconds(duration);
        bgmAudio.Stop();
        bgmAudio.clip = bgm[index];
        Debug.Log(bgmAudio.clip.name);
        bgmAudio.volume = 0.4f;
        bgmAudio.Play();
    }

    private IEnumerator GameOverTwo(float duration)
    {
        yield return new WaitForSeconds(duration);
        otherAudio.clip = other[5];
        otherAudio.Play();
    }

    private IEnumerator GrapplingSmall(float duration)
    {
        yield return new WaitForSeconds(duration);
        otherAudio.Stop();
    }
}
