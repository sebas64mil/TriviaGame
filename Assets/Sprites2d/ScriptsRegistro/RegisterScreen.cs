using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro
using System.Text.RegularExpressions;

public class RegisterScreen : MonoBehaviour
{
    [Header("UI Inputs")]
    public TMP_InputField inputName;
    public TMP_InputField inputEmail;

    [Header("UI Photo")]
    public RawImage photoPreview;

    [Header("UI Messages")]
    public TMP_Text messageText;

    [Header("Message Colors")]
    public Color successColor = Color.green;
    public Color errorColor = Color.red;
    public Color warningColor = Color.yellow;

    [Header("Object Actives")]
    public GameObject registerPanel;
    public GameObject continuePanel;

    [Header("Player Name Display")]
    public TMP_Text playerNameText; // <- Texto donde mostraremos el nombre guardado

    private Texture2D selectedPhoto;
    private WebCamTexture webCam;

    // Flag para saber si se registró
    private bool isRegistered = false;

    // -------------------
    // VALIDAR Y REGISTRAR
    // -------------------

    public void OnRegister()
    {
        string name = inputName.text.Trim();
        string email = inputEmail.text.Trim();

        // Validaciones del nombre
        if (string.IsNullOrEmpty(name))
        {
            ShowMessage("El nombre es obligatorio", errorColor);
            return;
        }

        if (name.Length < 3)
        {
            ShowMessage("El nombre debe tener al menos 3 caracteres", errorColor);
            return;
        }

        if (!IsValidName(name))
        {
            ShowMessage("El nombre solo puede contener letras y números", errorColor);
            return;
        }

        if (IsNameTaken(name))
        {
            ShowMessage("El nombre ya está en uso", errorColor);
            return;
        }

        // Validaciones del email
        if (string.IsNullOrEmpty(email))
        {
            ShowMessage("El correo es obligatorio", errorColor);
            return;
        }

        if (!IsValidEmail(email))
        {
            ShowMessage("Formato de correo inválido", errorColor);
            return;
        }

        if (selectedPhoto == null)
        {
            ShowMessage("Debes seleccionar o tomar una foto", errorColor);
            return;
        }

        // Si pasa todas las validaciones
        ShowMessage("Registro exitoso", successColor);
        isRegistered = true;

        // Guardamos el nombre en PlayerPrefs
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();

        // Mostrar el nombre de inmediato
        if (playerNameText != null)
            playerNameText.text = name;
    }

    // -------------------
    // FUNCIONES AUXILIARES
    // -------------------
    private bool IsValidName(string name)
    {
        // Solo letras y números
        string pattern = @"^[a-zA-Z0-9]+$";
        return Regex.IsMatch(name, pattern);
    }

    private bool IsNameTaken(string name)
    {
        // Aquí solo comprobamos el nombre guardado en PlayerPrefs
        string savedName = PlayerPrefs.GetString("PlayerName", "");
        return savedName.Equals(name, System.StringComparison.OrdinalIgnoreCase);
    }

    public void OnContinue()
    {
        if (!isRegistered)
        {
            ShowMessage("Primero debes registrarte antes de continuar", warningColor);
            return;
        }

        // Cambiar de panel
        registerPanel.SetActive(false);
        continuePanel.SetActive(true);

        // Recuperamos el nombre y lo mostramos
        if (playerNameText != null)
        {
            string savedName = PlayerPrefs.GetString("PlayerName", "Jugador");
            playerNameText.text = savedName;
        }
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    private void ShowMessage(string text, Color color)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.color = color;
        }
    }

    // -------------------
    // FOTO: DESDE GALERÍA
    // -------------------
    public void PickFromGallery()
    {
#if UNITY_ANDROID || UNITY_IOS
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512, false);
                if (texture != null)
                {
                    selectedPhoto = texture;
                    photoPreview.texture = texture;
                }
                else
                {
                    ShowMessage("No se pudo cargar la imagen", errorColor);
                }
            }
        }, "Selecciona una imagen", "image/*");
#else
        ShowMessage("La galería solo está disponible en Android/iOS", warningColor);
#endif
    }

    // -------------------
    // FOTO: DESDE CÁMARA
    // -------------------
    public void TakePhoto()
    {
#if UNITY_ANDROID || UNITY_IOS
        NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 512, false);
                if (texture != null)
                {
                    selectedPhoto = texture;
                    photoPreview.texture = texture;
                }
                else
                {
                    ShowMessage("No se pudo tomar la foto", errorColor);
                }
            }
        }, 512);
#else
        if (webCam == null)
        {
            webCam = new WebCamTexture();
            photoPreview.texture = webCam;
            webCam.Play();
            ShowMessage("Cámara activada. Pulsa de nuevo para capturar.", warningColor);
        }
        else if (webCam.isPlaying)
        {
            Texture2D photo = new Texture2D(webCam.width, webCam.height);
            photo.SetPixels(webCam.GetPixels());
            photo.Apply();

            selectedPhoto = photo;
            photoPreview.texture = photo;

            webCam.Stop();
            webCam = null;

            ShowMessage("Foto tomada con la webcam", successColor);
        }
#endif
    }
}
