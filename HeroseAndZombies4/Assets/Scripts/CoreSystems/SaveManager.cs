using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    private const string ENCRYPTION_KEY = "your_encryption_key_123";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // INT Kaydet/Yükle
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(EncryptKey(key), EncryptInt(value));
        PlayerPrefs.Save();
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.HasKey(EncryptKey(key)) ? 
            DecryptInt(PlayerPrefs.GetInt(EncryptKey(key))) : 
            defaultValue;
    }

    // OBJECT Kaydet/Yükle (JSON)
    public void SaveObject<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(EncryptKey(key), EncryptString(json));
    }

    public T LoadObject<T>(string key) where T : new()
    {
        if (!PlayerPrefs.HasKey(EncryptKey(key))) return new T();
        
        string json = DecryptString(PlayerPrefs.GetString(EncryptKey(key)));
        return JsonUtility.FromJson<T>(json);
    }

    // Şifreleme Fonksiyonları
    private string EncryptKey(string key) => key; // Gerçek projede şifrele
    private int EncryptInt(int value) => value ^ ENCRYPTION_KEY.GetHashCode();
    private int DecryptInt(int value) => value ^ ENCRYPTION_KEY.GetHashCode();
    private string EncryptString(string text) => text; // Gerçek şifreleme ekle
    private string DecryptString(string text) => text;

    public void SaveAll()
    {
        PlayerPrefs.Save();
    }

    public void DeleteSave(string key)
    {
        PlayerPrefs.DeleteKey(EncryptKey(key));
    }
}