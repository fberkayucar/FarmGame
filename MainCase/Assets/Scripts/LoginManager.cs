using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    // Inspector'dan atanacak UI elemanlar�
    [SerializeField] private TMP_InputField usernameInputField; // Kullan�c� ad�n� girece�i input field
    [SerializeField] private TextMeshProUGUI usernameDisplayText; // Kullan�c� ad�n� g�sterecek ekran �st� metin
    [SerializeField] private Button enterButton; // Enter butonu
    [SerializeField] private GameObject loginScreen; // Giri� ekran� GameObject�i (kapat�lacak)

    private const string USERNAME_KEY = "Username"; // PlayerPrefs i�in anahtar

    void Start()
    {
        // Daha �nce kullan�c� ad� kaydedilmi� mi kontrol et
        if (PlayerPrefs.HasKey(USERNAME_KEY))
        {
            // Kaydedilmi� kullan�c� ad�n� al ve do�rudan g�ster
            string savedUsername = PlayerPrefs.GetString(USERNAME_KEY);
            usernameDisplayText.text = savedUsername;
            loginScreen.SetActive(false); // Giri� ekran�n� gizle
        }
        else
        {
            // �lk kez a��l�yorsa giri� ekran�n� g�ster ve butona i�lev ekle
            loginScreen.SetActive(true);
            enterButton.onClick.AddListener(OnEnterButtonClicked); // Butona t�klama i�levi ekle
        }
    }

    // Enter butonuna bas�ld���nda �al��acak metod
    private void OnEnterButtonClicked()
    {
        string enteredUsername = usernameInputField.text.Trim(); // Input�tan kullan�c� ad�n� al ve bo�luklar� temizle

        // Kullan�c� ad� bo� mu kontrol et
        if (string.IsNullOrEmpty(enteredUsername))
        {
            return; // Bo�sa i�lemi durdur
        }

        // Kullan�c� ad�n� kaydet ve g�ster
        PlayerPrefs.SetString(USERNAME_KEY, enteredUsername);
        PlayerPrefs.Save(); // PlayerPrefs�i diske yaz
        usernameDisplayText.text = enteredUsername; // Ekran�n �st�nde g�ster
        loginScreen.SetActive(false); // Giri� ekran�n� gizle

        Debug.Log("Kullan�c� ad� kaydedildi: " + enteredUsername);
    }
}