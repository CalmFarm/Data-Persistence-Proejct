using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public InputField playerNameInput;
    void Start()
    {
        // 입력 필드 컴포넌트 가져오기
        playerNameInput = GameObject.Find("UserNameInput").GetComponent<InputField>();
    }

    public void StartGame()
    {
        // 입력한 사용자 이름 가져오기
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);

        // main scene load
        SceneManager.LoadScene("main");
    }
}
