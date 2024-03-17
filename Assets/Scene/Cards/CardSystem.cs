using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    // ScriptableObject�� ���ǵ� ī�� ������
    public SO card;

    [SerializeField] private TMP_Text manaBar;  // ī���� ���� �Ҹ� ǥ���ϴ� �ؽ�Ʈ
    [SerializeField] private Image spriteCard;  // ī���� �̹����� ǥ���ϴ� �̹��� UI

    void Start()
    {
        // ī�� ���� �ʱ�ȭ
        manaBar.text = card.manaBar.ToString();   // ī���� ���� �Ҹ� �ؽ�Ʈ�� ����
        spriteCard.sprite = card.playerPhoto;     // ī���� �̹����� �̹��� UI�� ����
    }
}
