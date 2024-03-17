using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    // ScriptableObject로 정의된 카드 데이터
    public SO card;

    [SerializeField] private TMP_Text manaBar;  // 카드의 마나 소모를 표시하는 텍스트
    [SerializeField] private Image spriteCard;  // 카드의 이미지를 표시하는 이미지 UI

    void Start()
    {
        // 카드 정보 초기화
        manaBar.text = card.manaBar.ToString();   // 카드의 마나 소모를 텍스트에 설정
        spriteCard.sprite = card.playerPhoto;     // 카드의 이미지를 이미지 UI에 설정
    }
}
