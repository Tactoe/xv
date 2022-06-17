using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SaveScene : MonoBehaviour
{
    public TMP_InputField MyField;
    public GameObject Err1;
    public GameObject Err2;
    [SerializeField]
    Toggle m_OnlineSave;
    [SerializeField]
    TMP_InputField m_SaveIdDisplayText;
    // Start is called before the first frame update
    void Start()
    {
        MyField.onSubmit.AddListener(sayChatMessage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckBeforeSave(){
        string m_stringToCheck = MyField.text;
        if(m_stringToCheck == ""){
            Err1.SetActive(true);
        }
        else if (PlayerPrefs.HasKey(m_stringToCheck)){
            Err2.SetActive(true);
        }
        else{
            PlayerPrefs.SetString("savesNames", PlayerPrefs.GetString("savesNames") + m_stringToCheck + ";" );
            fonctionSave();
        }
    }

    IEnumerator WaitForSaveId()
    {
        int timer = 100;
        while (SaveManager.Instance.SaveIdAfterSave == "" && timer >= 0)
        {
            m_SaveIdDisplayText.text ="Waiting for saveId...";
            timer--;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        if(timer <= 0){
            m_SaveIdDisplayText.text ="Server did not respond... Please try again later";
        }
        else{
            m_SaveIdDisplayText.text = "Your save id is :" + SaveManager.Instance.SaveIdAfterSave;   
        }
        SaveManager.Instance.SaveIdAfterSave = "";
    }
    
    void OnDisable()
    {
        m_SaveIdDisplayText.gameObject.SetActive(false);
        StopCoroutine(WaitForSaveId());
    }

    public void CleanErr(){
        Err1.SetActive(false);
        Err2.SetActive(false);
    }

    public void fonctionSave(){
        string m_stringToCheck = MyField.text;
        SaveManager.Instance.Save(m_stringToCheck, m_OnlineSave.isOn);
        if (m_OnlineSave.isOn)
        {
            m_SaveIdDisplayText.gameObject.SetActive(true);
            StartCoroutine(WaitForSaveId());
        }
        else{
            gameObject.SetActive(false);
        }
    }
    
    public void ForceSave(){
        Err2.SetActive(false);
        fonctionSave();
        // string m_stringToCheck = MyField.text;
        // Debug.Log("SAVE OK ");
        // SaveManager.Instance.Save(m_stringToCheck, m_OnlineSave.isOn);
        // Debug.Log(PlayerPrefs.GetString(m_stringToCheck));
        // gameObject.SetActive(false);
    }

 
    void sayChatMessage(string msg){
        CheckBeforeSave();
    }
}
