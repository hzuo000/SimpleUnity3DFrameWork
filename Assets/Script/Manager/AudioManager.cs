using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GameInterface
{
    private MusicID currentMusic;
    private bool _crossFading;//正在切换标志

    private AudioSource _activeMusic;//正在播放的bgm
    private AudioSource _inactiveMusic;//未播放的bgm
    private const float crossFadeRate = 1.5f;//切换时间

    [SerializeField] private List<AudioSource> Sounds;//把对应的音乐源拖入面板【顺序要和id对应】
    [SerializeField] private List<AudioSource> Music;
    /// <summary>
    /// 是否在播放音乐
    /// </summary>
    public bool IsMusic
    {
        get => GameManager.Record.systemRecord.bIsMusic;
        set
        {
            GameManager.Record.systemRecord.bIsMusic = value;
            foreach (AudioSource source in Music)
            {
                if (source != null)
                {
                    source.mute = !value;
                }
            }
        }
    }
    /// <summary>
    /// 是否在播放音效
    /// </summary>
    public bool IsVolume
    {
        get => GameManager.Record.systemRecord.bIsVolume;
        set
        {
            GameManager.Record.systemRecord.bIsVolume = value;
            if (value)
            {
                AudioListener.volume = 1.0f;
            }
            else
            {
                AudioListener.volume = 0.0f;
            }
        }
    }

    public override void StartUp()
    {
        ManagerType = GameManagerType.Audio;
        foreach (AudioSource source in Music)
        {
            if (source != null)
            {
                source.ignoreListenerPause = true;
                source.ignoreListenerVolume = true;
            }
        }
        currentMusic = MusicID.NULL;
        IsMusic = GameManager.Record.systemRecord.bIsMusic;
        IsVolume = GameManager.Record.systemRecord.bIsVolume;
        base.StartUp();
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="musicID"></param>
    public void PlayMusic(MusicID musicID)
    {
        if (_crossFading || musicID == currentMusic) return;
        StartCoroutine(CrossFading(Music[(int)musicID]));
        currentMusic = musicID;
    }
    /// <summary>
    /// 播放音效[按钮声音建议在FairyGUI里直接编辑]
    /// </summary>
    /// <param name="soundID"></param>
    public void PlaySound(SoundID soundID)
    {
        if (!IsVolume || (int)soundID >= Sounds.Count) return;

        Sounds[(int)soundID].Play();
    }
    /// <summary>
    /// 音乐渐入渐出
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private IEnumerator CrossFading(AudioSource source)
    {
        float maxVolume = .5f;
        _crossFading = true;
        if (_activeMusic == null)
        {
            _activeMusic = source;
            _activeMusic.volume = 0.0f;
            _activeMusic.Play();
            while (_activeMusic.volume < maxVolume)
            {
                _activeMusic.volume += crossFadeRate * Time.deltaTime;
                yield return null;
            }
            _activeMusic.volume = maxVolume;
        }
        else
        {
            _inactiveMusic = source;
            _inactiveMusic.volume = 0.0f;
            _inactiveMusic.Play();
            while (_activeMusic.volume > 0.0f)
            {
                _activeMusic.volume -= crossFadeRate * Time.deltaTime;
                _inactiveMusic.volume += crossFadeRate * Time.deltaTime;
                yield return null;
            }
            AudioSource temp = _activeMusic;
            _activeMusic = _inactiveMusic;
            _activeMusic.volume = maxVolume;

            _inactiveMusic = temp;
            _inactiveMusic.Stop();
        }
        _crossFading = false;
    }

    public void MuteAllVolum()//静音所有声音
    {
        AudioListener.volume = 0.0f;
        if (_activeMusic != null)
        {
            _activeMusic.volume = 0.0f;
        }
    }

    public void ResetAllVolum()//恢复所有声音
    {
        AudioListener.volume = 1.0f;
        if (_activeMusic != null)
        {
            _activeMusic.volume = 1.0f;
        }
    }
    public void StopActiveMusic()//暂停当前bgm
    {
        if (_activeMusic != null)
        {
            _activeMusic.Stop();
        }

    }
    /// <summary>
    /// 恢复当前bgm
    /// </summary>
    public void ReSetActiveMusic()
    {
        if (_activeMusic != null)
        {
            _activeMusic.Play();
        }

    }

    /// <summary>
    /// 暂时静音
    /// </summary>
    public void TempMuteMusic()
    {
        foreach (AudioSource source in Music)
        {
            if (source != null)
            {
                source.mute = true;
            }
        }
    }

    /// <summary>
    /// 恢复暂时静音
    /// </summary>
    public void ResetTempMute()
    {
        foreach (AudioSource source in Music)
        {
            if (source != null)
            {
                source.mute = !IsMusic;
            }
        }
    }
}
