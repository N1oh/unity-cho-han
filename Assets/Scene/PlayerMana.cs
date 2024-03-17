using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    // UI 요소들
    public Image manaBar;   // 마나 바 이미지
    public Text manaText;   // 마나 양을 표시하는 텍스트

    // 플레이어의 마나 변수들
    public float OurMana;   // 플레이어의 초기 마나
    public float maxMana = 10f;  // 플레이어의 최대 마나 양
    private float currentMana;   // 현재 마나 양

    // 게임이 시작할 때 초기화
    private void Start()
    {
        currentMana = 5;  // 시작 시 초기 마나를 5으로 설정
        UpdateManaBar();  // UI 업데이트
    }

    // 현재 마나 양을 반환하는 메서드
    public float GetCurrentMana()
    {
        return currentMana;
    }

    // 매 프레임마다 호출되는 업데이트 메서드
    private void Update()
    {
        // 마나를 조금씩 증가시키는 로직
        if (currentMana < OurMana)
        {
            float deltaMana = Time.deltaTime * 0.08f * OurMana;
            currentMana = Mathf.Clamp(currentMana + deltaMana, 0f, OurMana);
            UpdateManaBar();  // UI 업데이트
        }

        // 마나가 0 미만이면 0으로 설정
        if (currentMana < 0)
        {
            currentMana = 0;
            UpdateManaBar();  // UI 업데이트
        }

        manaText.text = "" + Mathf.FloorToInt(currentMana);  // UI에 현재 마나 양 표시

        // 3 미만인 경우 이미지 색을 붉은색으로 변경
        if (currentMana < 3)
        {
            ChangeImageColor(manaBar, Color.red);  // 마나 바 이미지 색 변경
            ChangeTextColor(manaText, Color.red);  // 마나 텍스트 색 변경
        }
        else
        {
            // 3 이상인 경우 이미지 색을 원래대로 변경
            ChangeImageColor(manaBar, Color.white);  // 마나 바 이미지 색 변경
            ChangeTextColor(manaText, Color.white);  // 마나 텍스트 색 변경
        }
    }

    // 주어진 마나전체양 만큼 마나를 감소시키는 메서드
    public void ReduceMana(float mana)
    {
        currentMana -= mana;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);  // 최소값과 최대값을 갖도록 클램핑
        UpdateManaBar();  // UI 업데이트
    }

    // UI를 업데이트하는 메서드
    private void UpdateManaBar()
    {
        manaBar.fillAmount = currentMana / maxMana;  // 마나 바의 fillAmount 업데이트
        manaText.text = Mathf.FloorToInt(currentMana).ToString();  // UI에 현재 마나 양 표시
    }

    // 마나 숫자 카운트 하는 메서드
    public void PlayCard(CardData card)
    {
        // 플레이어의 현재 마나가 카드의 비용 이상이면 마나를 감소시키고 카드를 플레이
        if (currentMana >= card.manaCost)
        {
            ReduceMana(card.manaCost);
            // 카드의 플레이에 필요한 추가 작업들을 수행
        }
        else
        {
            Debug.Log("Yeterli mana yok!");  // 마나가 부족한 경우 로그 출력
            // 마나가 부족하여 카드를 플레이하지 못하는 경우
        }
    }

    // 이미지의 색을 변경하는 메서드
    private void ChangeImageColor(Image image, Color color)
    {
        image.color = color;
    }

    // 텍스트의 색을 변경하는 메서드
    private void ChangeTextColor(Text text, Color color)
    {
        text.color = color;
    }
}