using UnityEngine;

public class SoundControl : MonoBehaviour
{
    private bool muteState;          // 是否靜音
    private float preVolume;         // 當前音量

    private AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.volume = 0;
        muteState = false;
        preVolume = aud.volume;
    }

    /// <summary>
    /// 音量改變
    /// </summary>
    /// <param name="newVolume">音量</param>
    public void VolumeChanged(float newVolume)
    {
        aud.volume = newVolume;
        muteState = false;
    }

    /// <summary>
    /// 靜音按鈕
    /// </summary>
    public void MuteClick()
    {
        muteState = !muteState;
        if (muteState)
        {
            preVolume = aud.volume;
            aud.volume = 0;
        }
        else
            aud.volume = preVolume;
    }
}
