using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScore;
    [SerializeField]
    private TextMeshProUGUI computerScore;
    [SerializeField]
    private TextMeshProUGUI result;

    int plyScore = 0;
    int compScore = 0;

    public void ConfigureScore(int score, bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            plyScore += score;
            playerScore.text = plyScore.ToString();
        }
        else
        {
            compScore += score;
            computerScore.text = compScore.ToString();
        }
    }

    private void OnEnable()
    {
        AIController.score += ConfigureScore;
    }

    private void OnDisable()
    {
        AIController.score -= ConfigureScore;
    }
}
