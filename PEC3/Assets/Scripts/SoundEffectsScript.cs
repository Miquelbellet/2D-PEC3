using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsScript : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSelectClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip jump1Clip;
    [SerializeField] private AudioClip jump2Clip;
    [SerializeField] private AudioClip jump3Clip;
    [SerializeField] private AudioClip jetpackClip;
    [SerializeField] private AudioClip airAttackClip;
    [SerializeField] private AudioClip bazookaClip;
    [SerializeField] private AudioClip uziFireClip;
    [SerializeField] private AudioClip axHitClip;
    [SerializeField] private AudioClip explosion1Clip;
    [SerializeField] private AudioClip explosion2Clip;
    [SerializeField] private AudioClip explosion3Clip;
    [SerializeField] private AudioClip winningClip;
    [SerializeField] private AudioClip hurt1Clip;
    [SerializeField] private AudioClip hurt2Clip;
    [SerializeField] private AudioClip hurt3Clip;
    [SerializeField] private AudioClip[] selectWormClips;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void ReducePitchAudio()
    {
        audioSource.pitch = 0.4f;
    }
    public void SetNormalPitchAudio()
    {
        audioSource.pitch = 1;
    }
    public void ButtonSelectClip()
    {
        audioSource.PlayOneShot(buttonSelectClip);
    }
    public void WalkClip()
    {
        if(!audioSource.isPlaying)
        {
            ReducePitchAudio();
            audioSource.PlayOneShot(walkClip);
        }
    }
    public void JumpClip()
    {
        SetNormalPitchAudio();
        var randNum = Random.Range(1, 4);
        if (randNum == 1) audioSource.PlayOneShot(jump1Clip);
        else if (randNum == 2) audioSource.PlayOneShot(jump2Clip);
        else if (randNum == 3) audioSource.PlayOneShot(jump3Clip);
    }
    public void JetpackClip()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(jetpackClip);
    }
    public void AirAttackClip()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(airAttackClip);
    }
    public void BazookaClip()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(bazookaClip);
    }
    public void UziFireClip()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(uziFireClip);
    }
    public void AxHitClip()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(axHitClip);
    }
    public void ExplosionClip()
    {
        var randNum = Random.Range(1, 4);
        if (randNum == 1) audioSource.PlayOneShot(explosion1Clip);
        else if (randNum == 2) audioSource.PlayOneShot(explosion2Clip);
        else if (randNum == 3) audioSource.PlayOneShot(explosion3Clip);
    }
    public void HurtClip()
    {
        var randNum = Random.Range(1, 4);
        if (randNum == 1) audioSource.PlayOneShot(hurt1Clip);
        else if (randNum == 2) audioSource.PlayOneShot(hurt2Clip);
        else if (randNum == 3) audioSource.PlayOneShot(hurt3Clip);
    }
    public void SelectWormClip()
    {
        var randNum = Random.Range(0, selectWormClips.Length);
        audioSource.PlayOneShot(selectWormClips[randNum]);
    }
    public void WinningClip()
    {
        audioSource.loop = true;
        audioSource.clip = winningClip;
        audioSource.Play();
    }
}
