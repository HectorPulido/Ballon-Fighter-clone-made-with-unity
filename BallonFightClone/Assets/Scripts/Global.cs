using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour {

    public static Global singleton;
    public Text scoreText;
    public GameObject gameOverGameObject;

    int score;

    private void Start()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
    public void GameOver()
    {
        StartCoroutine(IEGameOver());
    }
    IEnumerator IEGameOver()
    {
        yield return new WaitForSeconds(3);
        gameOverGameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

}
