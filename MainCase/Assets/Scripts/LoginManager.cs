using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    // Inspector'dan atanacak UI elemanlarý
    [SerializeField] private TMP_InputField usernameInputField; // Kullanýcý adýný gireceði input field
    [SerializeField] private TextMeshProUGUI usernameDisplayText; // Kullanýcý adýný gösterecek ekran üstü metin
    [SerializeField] private Button enterButton; // Enter butonu
    [SerializeField] private GameObject loginScreen; // Giriþ ekraný GameObject’i (kapatýlacak)

    private const string USERNAME_KEY = "Username"; // PlayerPrefs için anahtar

    void Start()
    {
        // Daha önce kullanýcý adý kaydedilmiþ mi kontrol et
        if (PlayerPrefs.HasKey(USERNAME_KEY))
        {
            // Kaydedilmiþ kullanýcý adýný al ve doðrudan göster
            string savedUsername = PlayerPrefs.GetString(USERNAME_KEY);
            usernameDisplayText.text = savedUsername;
            loginScreen.SetActive(false); // Giriþ ekranýný gizle
        }
        else
        {
            // Ýlk kez açýlýyorsa giriþ ekranýný göster ve butona iþlev ekle
            loginScreen.SetActive(true);
            enterButton.onClick.AddListener(OnEnterButtonClicked); // Butona týklama iþlevi ekle
        }
    }

    // Enter butonuna basýldýðýnda çalýþacak metod
    private void OnEnterButtonClicked()
    {
        string enteredUsername = usernameInputField.text.Trim(); // Input’tan kullanýcý adýný al ve boþluklarý temizle

        // Kullanýcý adý boþ mu kontrol et
        if (string.IsNullOrEmpty(enteredUsername))
        {
            return; // Boþsa iþlemi durdur
        }

        // Kullanýcý adýný kaydet ve göster
        PlayerPrefs.SetString(USERNAME_KEY, enteredUsername);
        PlayerPrefs.Save(); // PlayerPrefs’i diske yaz
        usernameDisplayText.text = enteredUsername; // Ekranýn üstünde göster
        loginScreen.SetActive(false); // Giriþ ekranýný gizle

        Debug.Log("Kullanýcý adý kaydedildi: " + enteredUsername);
    }
}