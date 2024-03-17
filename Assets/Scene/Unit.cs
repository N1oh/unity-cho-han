using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Unit 클래스는 추상 클래스로 선언되어 있습니다.
public abstract class Unit : MonoBehaviour
{
    // 유닛의 이름을 나타내는 문자열 변수
    public string unitName;

    // 유닛이 공격할 수 있는 대상들의 리스트
    public List<UnitType> targets;

    // 유닛의 소속 팀을 나타내는 열거형 변수
    public Team team;

    // 유닛의 유형을 나타내는 열거형 변수
    public UnitType unitType;

    // 공격 속도를 나타내는 변수
    public float attackSpeed;

    // 공격 범위를 나타내는 변수
    public float attackRange;

    // 유닛의 체력
    public int hitPoints;

    // 유닛이 가하는 피해량
    public int damage;

    // 현재 대상으로 지정된 유닛의 Transform
    public Transform currentTarget;

    // 공격 쿨다운 시간을 나타내는 변수
    public float attackCooldown;

    // 체력바를 나타내는 Unity UI Slider
    public Slider healthBar;


    // 공격 메서드
    public void Attack()
    {
        // 대상이 존재하는 경우에만 피해를 입힘
        if (currentTarget != null)
        {
            Unit targetUnit = currentTarget.GetComponent<Unit>();
            if (targetUnit != null && targetUnit.hitPoints > 0)
            {
                targetUnit.TakeDamage(damage);
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    // 피해를 입는 메서드
    public void TakeDamage(int takenDamage)
    {
        // 피해만큼 체력을 감소시키고 체력바를 업데이트
        hitPoints -= takenDamage;
        UpdateHealthBar();

        // 체력이 0 이하가 되면 게임 재시작 또는 유닛 파괴
        if (hitPoints <= 0)
        {
            if (gameObject.CompareTag("Base"))
            {
                
            }
            else
            {
                // 유닛 파괴
                Destroy(gameObject);
            }

            return;
        }
    }
        
    // 체력바를 업데이트하는 메서드
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = hitPoints;
        }
    }

}


// 유닛의 유형을 정의하는 열거형
public enum UnitType
{
    Ground,
    Tower
}

// 팀을 정의하는 열거형
public enum Team
{
    Blue,
    Red,
}
