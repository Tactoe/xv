using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MyVideoPlayer : MonoBehaviour
{
    private VideoPlayer m_video;
    public GameObject ButtonPlay;
    public GameObject ButtonPause;
    // Start is called before the first frame update
    void Start()
    {
        m_video = gameObject.GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayVideo(){
        ButtonPlay.SetActive(false);
        ButtonPause.SetActive(true);
        m_video.Play();
    }
    
    public void PauseVideo(){
        ButtonPlay.SetActive(true);
        ButtonPause.SetActive(false);
        m_video.Pause();
    }
}
