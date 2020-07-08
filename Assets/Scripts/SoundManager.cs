using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    #region singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion singleton

    public AudioSource audioSourceBGM;
    public AudioSource[] audioSourceEffects;

    public Sound[] bgmSounds;
    public Sound[] effectSounds;

    //이름에 맞는 오디오클립을 빈 오디오소스에 넣어서 재생한다
    public void PlaySE(string _name)
    {
        for(int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for(int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중");
            }

        }
    }

    public void PlayBGM(string _name)
    {
        audioSourceBGM.Stop();
        for(int i = 0; i < bgmSounds.Length; i++)
        {
            if (bgmSounds[i].name == _name)
            {
                audioSourceBGM.clip = bgmSounds[i].clip;
                audioSourceBGM.Play();
            }
        }
    }

    public void StopAllSE()
    {
        for(int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }
    
}
