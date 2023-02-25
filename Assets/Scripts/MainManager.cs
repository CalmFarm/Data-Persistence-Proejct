using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text PlayerNameText; // 이름을 표시할 Text 추가
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private int m_HighScore = 0;
    private string m_HighScorePlayerName = "";
    private string playerName;
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string HIGH_SCORE_PLAYER_NAME_KEY = "HighScorePlayerName";


    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        PlayerNameText.text = "Player name : " + playerName ;

        // PlayerPrefs에서 최고 점수와 플레이어 이름 로드
        m_HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        m_HighScorePlayerName = PlayerPrefs.GetString(HIGH_SCORE_PLAYER_NAME_KEY, "");
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (m_Points > m_HighScore)
        {
            // 현재 점수가 최고 점수보다 높으면 최고 점수 갱신
            m_HighScore = m_Points;
            m_HighScorePlayerName = playerName;

            // PlayerPrefs에 최고 점수와 플레이어 이름 저장
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, m_HighScore);
            PlayerPrefs.SetString(HIGH_SCORE_PLAYER_NAME_KEY, m_HighScorePlayerName);
        }

        // 현재 최고 점수와 플레이어 이름 표시
        PlayerNameText.text = $"High Score: {m_HighScore} by {m_HighScorePlayerName}";
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

}
