using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserPref : MonoBehaviour
{
    string m_SaveNames;
    public GameObject Pause;

    void Start()
    {
        if (PlayerPrefs.GetString("m_SaveNames") != null) {
            m_SaveNames = PlayerPrefs.GetString("m_SaveNames");
        } else {
            PlayerPrefs.SetString("m_SaveNames", "");
        }
    }

    public void Save() {
        PlayerPrefs.Save();
    }

    public void Reset() {
        PlayerPrefs.DeleteAll();
        Save();
    }

    public void quit(){
        AutoSave();
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        AutoSave();
    }

    public void ReturnToMenu(){
        AutoSave();
        SceneManager.LoadScene("titre_menu");
    }
    
    void AutoSave()
    {
        if (SceneManager.GetActiveScene().name == "titre_menu")
            return;
        SaveManager.Instance.Save("--autosave--", false);
		if (!PlayerPrefs.GetString("savesNames").Contains("--autosave--"))
	        PlayerPrefs.SetString("savesNames", PlayerPrefs.GetString("savesNames") + "--autosave--;" );

    }
    
    public void CreateNew(){
        PlayerPrefs.SetString("Current_Scene", "");
        SceneManager.LoadScene("Default");
    }
}
