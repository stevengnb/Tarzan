using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    [SerializeField] public List<AudioClip> audioo;
    private AudioSource audioSource;

    private void Awake() 
    {
        if (instance == null) 
        {
            audioSource = GetComponent<AudioSource>();
            instance = this;
        } else 
        {
            Destroy(gameObject);
        }   
    }
    public void ButtonClicked()
    {
        audioSource.clip = audioo[0];
        audioSource.Play();
    }
}
