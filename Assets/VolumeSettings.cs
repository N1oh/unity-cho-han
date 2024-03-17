using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    // 오디오 믹서와 슬라이더 컴포넌트에 대한 참조 변수들
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    // 게임 시작 시 실행되는 메서드
    private void Start()
    {
        // PlayerPrefs에 "musicVolume" 키가 저장되어 있는지 확인
        if (PlayerPrefs.HasKey("musicVolume"))
        {
           LoadVolume();// 저장된 볼륨 설정이 있으면 볼륨 불러옴
        }
        else
        {
            // 저장된 볼륨 설정이 없으면 기본 설정값으로 음악 및 SFX 볼륨 설정
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    // 음악 볼륨을 설정하는 메서드
    public void SetMusicVolume()
    {
        
        float volume = musicSlider.value; // 슬라이더에서 음악 볼륨 값을 가져옴
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);    // 믹서의 "music" 파라미터에 로그 스케일로 변환된 볼륨 값 적용
        PlayerPrefs.SetFloat("musicVolume", volume); // PlayerPrefs에 음악 볼륨 설정 저장
    }

    // SFX 볼륨을 설정하는 메서드
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value; // 슬라이더에서 SFX 볼륨 값을 가져옴
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);// 믹서의 "SFX" 파라미터에 로그 스케일로 변환된 볼륨 값 적용
        PlayerPrefs.SetFloat("SFXVolume", volume);// PlayerPrefs에 SFX 볼륨 설정 저장
    }

    // 저장된 볼륨을 로드하는 메서드
    private void LoadVolume()
    {
        // PlayerPrefs에서 저장된 음악 및 SFX 볼륨 설정 로드
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        // 로드된 볼륨을 믹서에 적용
        SetMusicVolume();
        SetSFXVolume();
    }
}