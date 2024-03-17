using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MuteSound : MonoBehaviour
{    
    [SerializeField] private AudioMixer audioMixer; // AudioMixer�� ���� ���� ����
    private Sprite soundOnImage; // ���� On�̹����� Off�̹����� ���� ����
    public Sprite soundOffImage; // ���� Off �̹���
    public Button button; // ��ư ���� ���� ����
    public Slider slider; // ���� �����̴� ����
    private bool isOn = true; // ���� �����̳� ȿ������ ���� �ִ��� ���θ� ��Ÿ���� ����, �ʱⰪ�� ���� ����
    void Start()
    {
        // ���� ��ư�� �̹����� �����ص�
        soundOnImage = button.image.sprite;
        // �����̴� �� ���� �̺�Ʈ�� ���� ������ �߰�
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    // ���� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
    public void MusicButtonClicked()
    {
        if (isOn)
        {
            // ���� On ���¸� Off�� ��ȯ
            button.image.sprite = soundOffImage;
            isOn = false;
            // AudioMixer�� "music" �Ķ���͸� -80���� �����Ͽ� ���Ұ�
            audioMixer.SetFloat("music", -80f);
        }
        else
        {
            // ���� Off ���¸� On���� ��ȯ
            button.image.sprite = soundOnImage;
            isOn = true;

            // AudioMixer�� "music" �Ķ���͸� 0���� �����Ͽ� ���Ұ� ����
            audioMixer.SetFloat("music", 0f);
        }
    }

    // SFX ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
    public void SFXButtonClicked()
    {
        if (isOn)
        {
            // ���� On ���¸� Off�� ��ȯ
            button.image.sprite = soundOffImage;
            isOn = false;

            // AudioMixer�� "SFX" �Ķ���͸� -80���� �����Ͽ� ���Ұ�
            audioMixer.SetFloat("SFX", -80f);
        }
        else
        {
            // ���� Off ���¸� On���� ��ȯ
            button.image.sprite = soundOnImage;
            isOn = true;

            // AudioMixer�� "SFX" �Ķ���͸� 0���� �����Ͽ� ���Ұ� ����
            audioMixer.SetFloat("SFX", 0f);
        }
    }

    // �����̴� ���� ����� �� ȣ��Ǵ� �޼���
    private void OnSliderValueChanged()
    {
        // ���Ұ� ���¿����� �����̴� �� ���� �� �̹����� On �̹����� ����
        if (!isOn)
        {
            button.image.sprite = soundOnImage;
            isOn = true;
        }
    }
}