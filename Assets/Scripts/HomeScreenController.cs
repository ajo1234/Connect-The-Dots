using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreenController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField rowIF;

    [SerializeField]
    private TMP_InputField columnIF;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button closeButton;

    private BoardSizeData boardSizeData;

    private void Start()
    {
        boardSizeData = GameObject.FindGameObjectWithTag("Player").GetComponent<BoardSizeData>();
        startButton.onClick.AddListener(OnStartButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);

    }

    private void OnStartButtonClick()
    {
        if (!string.IsNullOrEmpty(rowIF.text.ToString()) && !string.IsNullOrEmpty(columnIF.text.ToString()))
        {
            int row = int.Parse(rowIF.text.ToString(), System.Globalization.NumberStyles.Integer);
            int column = int.Parse(columnIF.text.ToString(), System.Globalization.NumberStyles.Integer);
            boardSizeData.rowValue = row;
            boardSizeData.columnValue = column; 
        }
        SceneManager.LoadScene("GameScene");
    }

    private void OnCloseButtonClick()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCloseButtonClick();
            }
        }
    }
}
