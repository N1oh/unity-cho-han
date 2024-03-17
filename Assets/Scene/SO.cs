using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// CreateAssetMenu Ư���� ����Ͽ� Unity �����Ϳ��� ��ũ���ͺ� ������Ʈ�� ������ �� ����� �޴��� �����մϴ�.
// fileName�� ������ ������ �⺻ �̸�, menuName�� �������� �޴��� ǥ�õǴ� �̸��Դϴ�.
[CreateAssetMenu(fileName = "AddCard", menuName = "Card")]
public class SO : ScriptableObject
{
    // ���ڿ� �����͸� �����ϴ� ����
    public string manaBar;

    // ��������Ʈ �����͸� �����ϴ� ����
    public Sprite playerPhoto;
}

