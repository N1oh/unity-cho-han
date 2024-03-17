using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Unit Ŭ������ �߻� Ŭ������ ����Ǿ� �ֽ��ϴ�.
public abstract class Unit : MonoBehaviour
{
    // ������ �̸��� ��Ÿ���� ���ڿ� ����
    public string unitName;

    // ������ ������ �� �ִ� ������ ����Ʈ
    public List<UnitType> targets;

    // ������ �Ҽ� ���� ��Ÿ���� ������ ����
    public Team team;

    // ������ ������ ��Ÿ���� ������ ����
    public UnitType unitType;

    // ���� �ӵ��� ��Ÿ���� ����
    public float attackSpeed;

    // ���� ������ ��Ÿ���� ����
    public float attackRange;

    // ������ ü��
    public int hitPoints;

    // ������ ���ϴ� ���ط�
    public int damage;

    // ���� ������� ������ ������ Transform
    public Transform currentTarget;

    // ���� ��ٿ� �ð��� ��Ÿ���� ����
    public float attackCooldown;

    // ü�¹ٸ� ��Ÿ���� Unity UI Slider
    public Slider healthBar;


    // ���� �޼���
    public void Attack()
    {
        // ����� �����ϴ� ��쿡�� ���ظ� ����
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

    // ���ظ� �Դ� �޼���
    public void TakeDamage(int takenDamage)
    {
        // ���ظ�ŭ ü���� ���ҽ�Ű�� ü�¹ٸ� ������Ʈ
        hitPoints -= takenDamage;
        UpdateHealthBar();

        // ü���� 0 ���ϰ� �Ǹ� ���� ����� �Ǵ� ���� �ı�
        if (hitPoints <= 0)
        {
            if (gameObject.CompareTag("Base"))
            {
                
            }
            else
            {
                // ���� �ı�
                Destroy(gameObject);
            }

            return;
        }
    }
        
    // ü�¹ٸ� ������Ʈ�ϴ� �޼���
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = hitPoints;
        }
    }

}


// ������ ������ �����ϴ� ������
public enum UnitType
{
    Ground,
    Tower
}

// ���� �����ϴ� ������
public enum Team
{
    Blue,
    Red,
}
