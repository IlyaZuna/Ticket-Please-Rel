using System;
using UnityEngine;
using System.Collections;

public class BusSoundController : MonoBehaviour
{
    [Header("Engine Sounds")]
    public AudioClip engineStartSound; // Звук запуска двигателя
    public AudioClip engineIdleSound;  // Звук работы на холостом ходу
    public AudioClip engineGaZ;
    public AudioClip engineRunningSound; // Звук движения
    public AudioClip engineStop;
    [SerializeField] private float delayBetweenSounds = 0.1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void PlayEngineStart()
    {
        if (engineStartSound != null)
        {
            audioSource.PlayOneShot(engineStartSound);
            // После запуска можно переключить на холостой ход с задержкой
            //Invoke("PlayEngineIdle", engineStartSound.length);
        }
        PlayEngineIdle();
    }

    public void PlayEngineIdle()
    {
        if (engineIdleSound != null && audioSource.clip != engineIdleSound)
        {
            audioSource.loop = true;
            audioSource.clip = engineIdleSound;
            audioSource.Play();
        }
    }

    public void PlayEngineRunning()
    {
       
        audioSource.loop = true;
        StartCoroutine(PlaySoundsOneAfterAnother());
   
            
          
        
    }
    public void PlayEngineGAZ()
    {
        if (engineGaZ != null && audioSource.clip != engineRunningSound)
        {
            audioSource.loop = false;
            audioSource.clip = engineGaZ;
            audioSource.Play();
        }
    }

    public void StopEngine()
    {
        audioSource.clip = engineStop;
        audioSource.Play();
        //audioSource.Stop();
        audioSource.loop = false;
    }
    
    private IEnumerator PlaySoundsOneAfterAnother()
    {
        audioSource.clip = engineGaZ;
        // Воспроизводим первый звук
        audioSource.Play();

        // Ждём окончания первого звука (+ небольшая задержка)
        yield return new WaitForSeconds(audioSource.clip.length + delayBetweenSounds);
        audioSource.clip = engineRunningSound;
   
        audioSource.Play();
       
    }
    public void Stop()
    {
        audioSource.clip = null;
    }
}