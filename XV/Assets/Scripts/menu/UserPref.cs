using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserPref : MonoBehaviour
{
    string savesNames;
    int nbSaves;
    public GameObject Pause;

    void Start()
    {
        if (PlayerPrefs.GetString("savesNames") != null) {
            savesNames = PlayerPrefs.GetString("savesNames");
        } else {
            PlayerPrefs.SetString("savesNames", "");
        }
        if (PlayerPrefs.GetInt("nbSaves") != null) {
            nbSaves = PlayerPrefs.GetInt("nbSaves");
        } else {
            PlayerPrefs.SetInt("nbSaves", 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        // PlayerPrefs.SetString("savesNames", "");
        // PlayerPrefs.SetInt("nbSaves", 0);
        PlayerPrefs.DeleteAll();
        Save();
    }

    public void quit(){
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("titre_menu");
    }
    
    
    public void CreateNew(){
        PlayerPrefs.SetString("Current_Scene", "");
        SceneManager.LoadScene("Default");
    }
}
