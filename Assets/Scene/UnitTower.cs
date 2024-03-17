using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTower : Unit
{
    // Update 메서드는 매 프레임마다 호출됩니다.
    private void Update()
    {
        // 공격 범위 내에서 대상을 찾아 설정합니다.
        FindTarget();

        // 유효한 대상이 있는지 확인합니다.
        if (currentTarget != null)
        {
            // 공격 쿨다운이 만료되었는지 확인합니다.
            if (attackCooldown <= 0f)
            {
                // Attack 메서드를 실행합니다.
                Attack();

                // 공격 쿨다운을 재설정합니다.
                attackCooldown = attackSpeed;
            }

            // 공격 쿨다운을 업데이트합니다.
            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    // FindTarget 메서드는 공격 범위 내에서 적을 찾는 역할을 합니다.
    public void FindTarget()
    {
        // 특정 공격 범위 내의 모든 콜라이더를 가져옵니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

        // 찾은 각 콜라이더에 대해 반복합니다.
        foreach (Collider collider in colliders)
        {
            // 콜라이더가 Unit 컴포넌트를 가지고 있는지 확인합니다.
            if (collider.TryGetComponent(out Unit enemy))
            {
                // 대상의 팀이 에이전트의 팀과 다르고 에이전트가 공격 가능한 대상인 경우.
                if (team != enemy.team && targets.Contains(enemy.unitType))
                {
                    // 찾은 적을 현재 대상으로 설정합니다.
                    currentTarget = collider.transform;
                    return;
                }
            }
        }

        // 대상을 찾지 못하면 currentTarget을 null로 설정합니다.
        currentTarget = null;
    }

    // OnDrawGizmosSelected는 에디터에서 지저분한 것을 그리기 위해 사용되는 Unity 콜백입니다.
    private void OnDrawGizmosSelected()
    {
        // 에디터에서 빨간색 와이어 스피어로 공격 범위를 시각화합니다.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
