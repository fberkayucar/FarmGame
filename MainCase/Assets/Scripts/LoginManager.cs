using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField; 
    [SerializeField] private TextMeshProUGUI usernameDisplayText; 
    [SerializeField] private Button enterButton; 
    [SerializeField] private GameObject loginScreen; 

    private const string USERNAME_KEY = "Username";

    void Start()
    {
        if (PlayerPrefs.HasKey(USERNAME_KEY))
        {
            string savedUsername = PlayerPrefs.GetString(USERNAME_KEY);
            usernameDisplayText.text = savedUsername;
            loginScreen.SetActive(false); 
        }
        else
        {
            loginScreen.SetActive(true);
            enterButton.onClick.AddListener(OnEnterButtonClicked); 
        }
    }

    private void OnEnterButtonClicked()
    {
        string enteredUsername = usernameInputField.text.Trim(); 
        if (string.IsNullOrEmpty(enteredUsername))
        {
            return; 
        }

        PlayerPrefs.SetString(USERNAME_KEY, enteredUsername);
        PlayerPrefs.Save();
        usernameDisplayText.text = enteredUsername; 
        loginScreen.SetActive(false); 

        Debug.Log("Kullanýcý adý kaydedildi: " + enteredUsername);
    }
}