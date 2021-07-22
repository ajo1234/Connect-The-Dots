using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScore;
    [SerializeField]
    private TextMeshProUGUI computerScore;
    [SerializeField]
    private TextMeshProUGUI result;
    [SerializeField]
    private TextMeshProUGUI turn;
    [SerializeField]
    private Button quitButton;

    int plyScore = 0;
    int compScore = 0;

    string playerScoreText = "Player Score: ";
    string compScoreText = "Computer Score: ";
    string yourTurnText = "YOUR TURN";
    string compTurnText = "COMPUTER TURN";

    private void Start()
    {
        quitButton.onClick.AddListener(OnCloseButtonClick);
    }

    void OnCloseButtonClick()
    {
        SceneManager.LoadScene("HomeScene");
    }

    private void OnEnable()
    {
        AIController.Score += ConfigureScore;
        BoardController.GameEnded += ShowGameResult;
        BoardController.TurnChanged += ConfigureCurrentPlayerText;
    }

    private void OnDisable()
    {
        AIController.Score -= ConfigureScore;
        BoardController.GameEnded -= ShowGameResult;
        BoardController.TurnChanged -= ConfigureCurrentPlayerText;
    }
    private void ConfigureScore(int score, bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            plyScore += score;
            playerScore.text = playerScoreText + plyScore.ToString();
        }
        else
        {
            compScore += score;
            computerScore.text = compScoreText + compScore.ToString();
        }
    }

    private void ConfigureCurrentPlayerText()
    {
        if (BoardController.isPlayerTurn)
        {
            turn.text = yourTurnText;
            turn.color = Color.blue;
        }
        else
        {
            turn.text = compTurnText;
            turn.color = Color.green;
        }
    }

    private void ShowGameResult()
    {
        result.gameObject.SetActive(true);
        turn.gameObject.SetActive(false);
        if (plyScore > compScore)
        {
            result.text = "YOU WON!!";
        } 
        else if (plyScore < compScore)
        {
            result.text = "BETTER LUCK NEXT TIME";
        }
        else
        {
            result.text = "DRAW !!";
        }
    }

}
