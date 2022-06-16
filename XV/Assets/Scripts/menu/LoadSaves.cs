using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class LoadSaves : MonoBehaviour
{
    public GameObject SavePrefab;
    // public int IndexSave;

    private string m_saves;
    private string[] m_aSaves;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("-----------------------------------------------");
        // Debug.Log(gameObject.name);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        // PlayerPrefs.SetInt("nbSaves", 9);
        // PlayerPrefs.SetString("savesNames", "atelier1;atelier2;balbla;12atelier1;usine;usinedetonton;test7;miaou;test9");
        // string[] lol = PlayerPrefs.GetString("savesNames").Split(";");
        // foreach (string lilol in lol)
        // {
        //     PlayerPrefs.SetString(lilol, lilol+"alablblabalablablablalabalablablalbalbalalabablalbabab");
        // }
        m_saves = PlayerPrefs.GetString("savesNames");
        Debug.Log(m_saves);
        m_aSaves = m_saves.Split(";",  System.StringSplitOptions.RemoveEmptyEntries);


        foreach(string line in m_aSaves)
        {
            GameObject tmp =  Instantiate(SavePrefab, transform);
            tmp.GetComponentInChildren<TextMeshProUGUI>().SetText("  " +line);
            tmp.GetComponent<Button>().onClick.AddListener(() => PreLoadSave(line));
        }
        if(m_aSaves.Length > 0)
            PreLoadSave(m_aSaves[0]);
    }

    public void PreLoadSave(string name){

        // int index = i_index;
        Debug.Log("in preload de  = " + name);
        GameObject.Find("Button_launch").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Button_del").GetComponent<Button>().onClick.RemoveAllListeners();

        GameObject.Find("name_save").GetComponent<TextMeshProUGUI>().SetText("  " +name);
        GameObject.Find("Button_launch").GetComponent<Button>().onClick.AddListener(() => LoadSave(name));
        GameObject.Find("Button_del").GetComponent<Button>().onClick.AddListener(() => DelSave(name));
    }


    public void LoadSave(string name){
        if (PlayerPrefs.GetString(name) == "")
            return;
        Debug.Log("Load scene index " + name);
        Debug.Log("data  = " + PlayerPrefs.GetString(name));
        //SaveManager.Instance.Load(name);
        PlayerPrefs.SetString("Current_Scene", name);
        Time.timeScale = 1;
        SceneManager.LoadScene("Default");
    }


    
    public void DelSave(string name){
        Debug.Log("DEL scene index " + name);
        Debug.Log("data  = " + PlayerPrefs.GetString(name));
        Debug.Log("PlayerPrefs.GetString(savesNames =  " + PlayerPrefs.GetString("savesNames"));
        PlayerPrefs.SetString(name, "");
        PlayerPrefs.DeleteKey(name);
        string text = PlayerPrefs.GetString("savesNames");
        text = text.Replace(name+";", "");
        PlayerPrefs.SetString("savesNames", text);
        Debug.Log("PlayerPrefs.GetString(savesNames =  " + PlayerPrefs.GetString("savesNames"));

        Debug.Log("has ?" + PlayerPrefs.HasKey(name));

        PlayerPrefs.Save();

        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        m_saves = PlayerPrefs.GetString("savesNames");
        Debug.Log(m_saves);
        m_aSaves = m_saves.Split(";",  System.StringSplitOptions.RemoveEmptyEntries);
        foreach(string line in m_aSaves)
        {
            GameObject tmp =  Instantiate(SavePrefab, transform);
            tmp.GetComponentInChildren<TextMeshProUGUI>().SetText("  " +line);
            tmp.GetComponent<Button>().onClick.AddListener(() => PreLoadSave(line));
        }
        if(m_aSaves.Length > 0)
            PreLoadSave(m_aSaves[0]);

    }
}
