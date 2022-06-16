using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserPref : MonoBehaviour
{
    string m_SaveNames;
    int m_nbSaves;
    public GameObject Pause;

    void Start()
    {
        if (PlayerPrefs.GetString("m_SaveNames") != null) {
            m_SaveNames = PlayerPrefs.GetString("m_SaveNames");
        } else {
            PlayerPrefs.SetString("m_SaveNames", "");
        }
        if (PlayerPrefs.GetInt("m_nbSaves") != 0) {
            m_nbSaves = PlayerPrefs.GetInt("m_nbSaves");
        } else {
            PlayerPrefs.SetInt("m_nbSaves", 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "titre_menu")
        {
            if(Pause.activeInHierarchy)
                Pause.SetActive(false);
            else
                Pause.SetActive(true);
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
        Application.Quit();
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("titre_menu");
    }
    
    
    public void CreateNew(){
        PlayerPrefs.SetString("Current_Scene", "");
        SceneManager.LoadScene("Default");
    }
}
