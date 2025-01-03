using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
    public Button startButton;
    public Button settingsButton;
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject scoreCanvas;

    private float VerticalSpeed;
    private float PipeSpawnCountdown;
    private GameObject PipesHolder;
    private int PipeCount;
    private int score;
    private int HighScore = 0;

    // Game state control
    private bool isGameActive = false;
    private bool isInMainMenu = true;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);

        score = 0;
        scoreText.text = "Score: " + score.ToString();
        PipeCount = 0;

        PipesHolder = new GameObject("PipesHolder");
        PipesHolder.transform.parent = this.transform;

        scoreCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);

        VerticalSpeed = 0;
        Bird.transform.position = Vector3.up * 5;
        PipeSpawnCountdown = 0;
    }

    void Update()
    {
        // If the game is in the main menu, do nothing
        if (isInMainMenu) return;

        // Check for Esc key to return to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
            return;
        }

        if (!isGameActive)
        {
            VerticalSpeed = Mathf.Sin(Time.time * 2f) * Jump;
            Bird.transform.position += Vector3.up * VerticalSpeed * Time.deltaTime;

            // Prevent the bird from falling below y = 1
            if (Bird.transform.position.y < 1)
            {
                Bird.transform.position = new Vector3(Bird.transform.position.x, 1, Bird.transform.position.z);
            }

            return;
        }

        VerticalSpeed += -Gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            VerticalSpeed = 0;
            VerticalSpeed += Jump;
        }

        if (Bird.transform.position.y <= 0)
        {
            StartGame();
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
                    if (score > HighScore)
                    {
                        HighScore = score;
                        highScore.text = "High Score: " + HighScore.ToString();
                    }
                }

                if (pipe.position.x < -30)
                {
                    Destroy(pipe.gameObject);
                }
            }
        }
    }

    private void StartGame()
    {
        isInMainMenu = false;
        isGameActive = true;
        mainMenuCanvas.SetActive(false);
        scoreCanvas.SetActive(true);
    }

    private void OpenSettings()
    {
        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        // Stop the game and go to the main menu
        isGameActive = false;
        isInMainMenu = true;

        // Hide the score canvas and show the main menu
        scoreCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);

        // Reset the game variables (optional, based on your preference)
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        PipesHolder.transform.position = Vector3.zero;
        Bird.transform.position = Vector3.up * 5;
        VerticalSpeed = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
         StartGame();

    }
}
