using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public AudioSource swordHit;
    public AudioSource axeHit;
    public AudioSource stamp;
    public AudioSource kidHit;
    public AudioSource playerHit;
    public AudioSource enemyHit;
    public AudioSource newWave;
    public AudioSource backgroundMusic;

    void Awake()
    {
        Instance = this;
    }  
}
