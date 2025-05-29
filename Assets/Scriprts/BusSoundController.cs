using System;
using UnityEngine;
using System.Collections;

public class BusSoundController : MonoBehaviour
{
    [Header("Engine Sounds")]
    public AudioClip engineStartSound; // ���� ������� ���������
    public AudioClip engineIdleSound;  // ���� ������ �� �������� ����
    public AudioClip engineGaZ;
    public AudioClip engineRunningSound; // ���� ��������
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
            // ����� ������� ����� ����������� �� �������� ��� � ���������
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
        // ������������� ������ ����
        audioSource.Play();

        // ��� ��������� ������� ����� (+ ��������� ��������)
        yield return new WaitForSeconds(audioSource.clip.length + delayBetweenSounds);
        audioSource.clip = engineRunningSound;
   
        audioSource.Play();
       
    }
    public void Stop()
    {
        audioSource.clip = null;
    }
}