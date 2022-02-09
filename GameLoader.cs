using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public HighScore highScore;

    void Start()
    {
        AudioManager.Instance.backgroundMusic.Play();
        highScore.LoadResult();
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("FinalScene_piotrek");
    }
}
