using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreenController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI row;

    [SerializeField]
    private TextMeshProUGUI column;

    [SerializeField]
    private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        //if (int.TryParse(row.text, out int result))
        PlayerPrefs.SetInt("Row", Convert.ToInt32(row.text));
        if (int.TryParse(column.text, out int result))
            PlayerPrefs.SetInt("Column", result);
        SceneManager.LoadScene("GameScene");
    }
}
