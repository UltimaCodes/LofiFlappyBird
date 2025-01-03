using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlappyController : MonoBehaviour
{
    public GameObject PipePrefab;
    public GameObject Bird;
    public float Gravity = 30;
    public float Jump = 10;
    public float PipeSpawnInterval = 2;
    public float PipesSpeed = 5;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScore;

    private float VerticalSpeed;
    private float PipeSpawnCountdown;
    private GameObject PipesHolder;
    private int PipeCount;
    private int score;
    private int HighScore = 0;

    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        PipeCount = 0;
        Destroy(PipesHolder);

        PipesHolder = new GameObject("PipesHolder");
        PipesHolder.transform.parent = this.transform;

        VerticalSpeed = 0;
        Bird.transform.position = Vector3.up * 5;
        PipeSpawnCountdown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        VerticalSpeed += -Gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            VerticalSpeed = 0;
            VerticalSpeed += Jump;
        }

        Bird.transform.position += Vector3.up * VerticalSpeed * Time.deltaTime;

        PipeSpawnCountdown -= Time.deltaTime;

        if (PipeSpawnCountdown <= 0)
        {
            PipeSpawnCountdown = PipeSpawnInterval;
            GameObject pipe = Instantiate(PipePrefab);
            pipe.transform.parent = PipesHolder.transform;
            pipe.transform.name = (++PipeCount).ToString();

            pipe.transform.position += Vector3.right * 30;
            pipe.transform.position += Vector3.up * Mathf.Lerp(4, 9, Random.value);
        }

        PipesHolder.transform.position += Vector3.left * PipesSpeed * Time.deltaTime;

        foreach (Transform pipe in PipesHolder.transform)
        {
            if (pipe.position.x < 0)
            {
                int pipeId = int.Parse(pipe.name);
                if (pipeId > score)
                {
                    score = pipeId;
                    scoreText.text = "Score: " + score.ToString();
                    {
                        if (score > HighScore)
                        {
                            HighScore = score;
                            highScore.text = "High Score: " + HighScore.ToString();
                        }
                    }
                }

                if (pipe.position.x < -30)
                {
                    Destroy(pipe.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Start();
    }
}
