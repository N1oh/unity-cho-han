using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// CreateAssetMenu 특성을 사용하여 Unity 에디터에서 스크립터블 오브젝트를 생성할 때 사용할 메뉴를 정의합니다.
// fileName은 생성된 파일의 기본 이름, menuName은 에디터의 메뉴에 표시되는 이름입니다.
[CreateAssetMenu(fileName = "AddCard", menuName = "Card")]
public class SO : ScriptableObject
{
    // 문자열 데이터를 저장하는 변수
    public string manaBar;

    // 스프라이트 데이터를 저장하는 변수
    public Sprite playerPhoto;
}

