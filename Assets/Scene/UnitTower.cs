using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTower : Unit
{
    // Update �޼���� �� �����Ӹ��� ȣ��˴ϴ�.
    private void Update()
    {
        // ���� ���� ������ ����� ã�� �����մϴ�.
        FindTarget();

        // ��ȿ�� ����� �ִ��� Ȯ���մϴ�.
        if (currentTarget != null)
        {
            // ���� ��ٿ��� ����Ǿ����� Ȯ���մϴ�.
            if (attackCooldown <= 0f)
            {
                // Attack �޼��带 �����մϴ�.
                Attack();

                // ���� ��ٿ��� �缳���մϴ�.
                attackCooldown = attackSpeed;
            }

            // ���� ��ٿ��� ������Ʈ�մϴ�.
            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    // FindTarget �޼���� ���� ���� ������ ���� ã�� ������ �մϴ�.
    public void FindTarget()
    {
        // Ư�� ���� ���� ���� ��� �ݶ��̴��� �����ɴϴ�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

        // ã�� �� �ݶ��̴��� ���� �ݺ��մϴ�.
        foreach (Collider collider in colliders)
        {
            // �ݶ��̴��� Unit ������Ʈ�� ������ �ִ��� Ȯ���մϴ�.
            if (collider.TryGetComponent(out Unit enemy))
            {
                // ����� ���� ������Ʈ�� ���� �ٸ��� ������Ʈ�� ���� ������ ����� ���.
                if (team != enemy.team && targets.Contains(enemy.unitType))
                {
                    // ã�� ���� ���� ������� �����մϴ�.
                    currentTarget = collider.transform;
                    return;
                }
            }
        }

        // ����� ã�� ���ϸ� currentTarget�� null�� �����մϴ�.
        currentTarget = null;
    }

    // OnDrawGizmosSelected�� �����Ϳ��� �������� ���� �׸��� ���� ���Ǵ� Unity �ݹ��Դϴ�.
    private void OnDrawGizmosSelected()
    {
        // �����Ϳ��� ������ ���̾� ���Ǿ�� ���� ������ �ð�ȭ�մϴ�.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
