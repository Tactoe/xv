using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MyVideoPlayer : MonoBehaviour
{
    private VideoPlayer m_Video;
    public GameObject ButtonPlay;
    public GameObject ButtonPause;
    // Start is called before the first frame update
    void Start()
    {
        m_Video = gameObject.GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayVideo(){
        ButtonPlay.SetActive(false);
        ButtonPause.SetActive(true);
        m_Video.Play();
    }
    
    public void PauseVideo(){
        ButtonPlay.SetActive(true);
        ButtonPause.SetActive(false);
        m_Video.Pause();
    }
}
