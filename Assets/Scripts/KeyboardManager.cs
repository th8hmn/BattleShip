using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class KeyboardManager : MonoBehaviour
{
    private TouchScreenKeyboard keyboard;
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordInputField2;
    public TMP_InputField playernameInputField;
    public TMP_InputField playernameInputField2;

    //スマホかどうかを保存しておく
    public bool IsSmartPhone { get; private set; } = false;

    [DllImport("__Internal")]
    private static extern void CheckPlatform();

    // Start is called before the first frame update
    void Start()
    {

#if (UNITY_WEBGL && !UNITY_EDITOR)
        CheckPlatform();
#endif
        //passwordInputField = this.gameObject.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null)
        {
            if (passwordInputField.isFocused)
            {
                passwordInputField.text = this.keyboard.text;
            }
            else if (playernameInputField.isFocused)
            {
                playernameInputField.text = this.keyboard.text;
            }

            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                this.keyboard.active = false;
            }
        }

    }

    public void SetPassword()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            this.keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // iOS
            this.keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // Windows
        }

        // webglの場合自動入力
#if (UNITY_WEBGL && !UNITY_EDITOR)
        if (!TouchScreenKeyboard.isSupported)
        {
            passwordInputField.text = GetSomeInput(passwordInputField.text);
            passwordInputField2.text = GetSomeInput(passwordInputField2.text);
            //playernameInputField.text = GetSomeName(playernameInputField.text);
        }
#endif
    }

    // webglの場合自動入力
    public void SetPlayerName()
    {

#if (UNITY_WEBGL && !UNITY_EDITOR)
        if (!TouchScreenKeyboard.isSupported)
        {
            //passwordInputField.text = GetSomeInput(passwordInputField.text);
            playernameInputField.text = GetSomeName(playernameInputField.text);
            playernameInputField2.text = GetSomeName(playernameInputField2.text);
        }
#endif
    }

    public void CloseKeyboard()
    {
        if (keyboard != null)
        {
            this.keyboard.active = false;
            this.keyboard = null;
        }
    }

    //JavaScriptから呼び出すメソッド
    public void setSmartPhoneMode()
    {
        IsSmartPhone = true;
    }

    // ルームの設定
    private string GetSomeInput(string text)
    {
        string someInput = "";
        if (text == "")
        {
            someInput = "RoomA";
        }
        else
        {
            switch (text)
            {
                case "RoomA":
                    someInput = "RoomB";
                    break;
                case "RoomB":
                    someInput = "RoomC";
                    break;
                case "RoomC":
                    someInput = "RoomA";
                    break;
            };
        }

        return someInput;
    }

    // プレイヤーネームの設定
    private string GetSomeName(string text)
    {
        string someInput = "";
        if (text == "")
        {
            someInput = "PlayerA";
        }
        else
        {
            switch (text)
            {
                case "PlayerA":
                    someInput = "PlayerB";
                    break;
                case "PlayerB":
                    someInput = "PlayerC";
                    break;
                case "PlayerC":
                    someInput = "PlayerA";
                    break;
            };
        }

        return someInput;
    }
}
