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
    private TMP_InputField rowIF;

    [SerializeField]
    private TMP_InputField columnIF;

    [SerializeField]
    private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        int num = int.Parse(rowIF.text.ToString(), System.Globalization.NumberStyles.Integer);
        PlayerPrefs.SetInt("Row", num);
        SceneManager.LoadScene("GameScene");
    }
}
