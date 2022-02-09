using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public string savePath;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/Highscore.dat";
    }

    public void StartCountingPoints()
    {
        score = 0;
        StartCoroutine(AddPointsCoroutine());
    }

    public void KillEnemy()
    {
        score += 5;
    }

    private IEnumerator AddPointsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            score += 1;
            scoreText.text = score.ToString();
        }
    }

    public void SaveResult()
    {
        StopCoroutine(AddPointsCoroutine());
        File.WriteAllText(savePath, score.ToString());
    }

    public void LoadResult()
    {
        if (Directory.Exists(savePath))
            scoreText.text = File.ReadAllText(savePath);
    }
}
