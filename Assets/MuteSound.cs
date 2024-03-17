using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MuteSound : MonoBehaviour
{    
    [SerializeField] private AudioMixer audioMixer; // AudioMixer에 대한 참조 변수
    private Sprite soundOnImage; // 음향 On이미지와 Off이미지에 대한 변수
    public Sprite soundOffImage; // 음향 Off 이미지
    public Button button; // 버튼 대한 참조 변수
    public Slider slider; // 음향 슬라이더 변수
    private bool isOn = true; // 현재 음악이나 효과음이 켜져 있는지 여부를 나타내는 변수, 초기값은 켜진 상태
    void Start()
    {
        // 현재 버튼의 이미지를 저장해둠
        soundOnImage = button.image.sprite;
        // 슬라이더 값 변경 이벤트에 대한 리스너 추가
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    // 음악 버튼이 클릭되었을 때 호출되는 메서드
    public void MusicButtonClicked()
    {
        if (isOn)
        {
            // 현재 On 상태면 Off로 전환
            button.image.sprite = soundOffImage;
            isOn = false;
            // AudioMixer의 "music" 파라미터를 -80으로 설정하여 음소거
            audioMixer.SetFloat("music", -80f);
        }
        else
        {
            // 현재 Off 상태면 On으로 전환
            button.image.sprite = soundOnImage;
            isOn = true;

            // AudioMixer의 "music" 파라미터를 0으로 설정하여 음소거 해제
            audioMixer.SetFloat("music", 0f);
        }
    }

    // SFX 버튼이 클릭되었을 때 호출되는 메서드
    public void SFXButtonClicked()
    {
        if (isOn)
        {
            // 현재 On 상태면 Off로 전환
            button.image.sprite = soundOffImage;
            isOn = false;

            // AudioMixer의 "SFX" 파라미터를 -80으로 설정하여 음소거
            audioMixer.SetFloat("SFX", -80f);
        }
        else
        {
            // 현재 Off 상태면 On으로 전환
            button.image.sprite = soundOnImage;
            isOn = true;

            // AudioMixer의 "SFX" 파라미터를 0으로 설정하여 음소거 해제
            audioMixer.SetFloat("SFX", 0f);
        }
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    private void OnSliderValueChanged()
    {
        // 음소거 상태에서만 슬라이더 값 변경 시 이미지를 On 이미지로 변경
        if (!isOn)
        {
            button.image.sprite = soundOnImage;
            isOn = true;
        }
    }
}