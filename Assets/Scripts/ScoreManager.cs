using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public Transform player;
    private int score = 0;
    private float lastY = 0f;
    private int lastScoreY = 0;
    public int scoreMultiplier = 1;
    void Start()
    {
        if (player != null)
            lastY = player.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            int currentY = Mathf.FloorToInt(player.position.y);
            int deltaScore = currentY - lastScoreY;
            if (deltaScore > 0)
            {
                score += deltaScore * scoreMultiplier;
                lastScoreY = currentY;
            }
        }
        if (scoreText != null)
            scoreText.text = Convert.ToString(score);
    }
}