using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    // ����� �ͼ��� �����̴� ������Ʈ�� ���� ���� ������
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    // ���� ���� �� ����Ǵ� �޼���
    private void Start()
    {
        // PlayerPrefs�� "musicVolume" Ű�� ����Ǿ� �ִ��� Ȯ��
        if (PlayerPrefs.HasKey("musicVolume"))
        {
           LoadVolume();// ����� ���� ������ ������ ���� �ҷ���
        }
        else
        {
            // ����� ���� ������ ������ �⺻ ���������� ���� �� SFX ���� ����
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    // ���� ������ �����ϴ� �޼���
    public void SetMusicVolume()
    {
        
        float volume = musicSlider.value; // �����̴����� ���� ���� ���� ������
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);    // �ͼ��� "music" �Ķ���Ϳ� �α� �����Ϸ� ��ȯ�� ���� �� ����
        PlayerPrefs.SetFloat("musicVolume", volume); // PlayerPrefs�� ���� ���� ���� ����
    }

    // SFX ������ �����ϴ� �޼���
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value; // �����̴����� SFX ���� ���� ������
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);// �ͼ��� "SFX" �Ķ���Ϳ� �α� �����Ϸ� ��ȯ�� ���� �� ����
        PlayerPrefs.SetFloat("SFXVolume", volume);// PlayerPrefs�� SFX ���� ���� ����
    }

    // ����� ������ �ε��ϴ� �޼���
    private void LoadVolume()
    {
        // PlayerPrefs���� ����� ���� �� SFX ���� ���� �ε�
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        // �ε�� ������ �ͼ��� ����
        SetMusicVolume();
        SetSFXVolume();
    }
}