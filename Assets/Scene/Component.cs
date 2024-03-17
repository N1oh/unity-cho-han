using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Component : Unit
{
    private NavMeshAgent agent; // NavMeshAgent를 사용하기 위한 변수
    public int mana; // 마나를 나타내는 변수
    public int unitAmount; // 유닛 수를 나타내는 변수
    public float movementSpeed; // 이동 속도를 나타내는 변수
    public float sightRange; // 시야 범위를 나타내는 변수
    public GameObject rightPath; // 오른쪽 경로를 나타내는 변수
    public GameObject leftPath; // 왼쪽 경로를 나타내는 변수
    private Animator anim; // Animator 컴포넌트를 사용하기 위한 변수
    public float rotationSpeed = 5f; // 회전 속도를 나타내는 변수
    public AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Animator 컴포넌트를 Awake 시점에서 가져옴
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트를 Start 시점에서 가져옴
    }

    private void Update()
    {
        FindTarget(); // 대상을 찾는 메서드 호출
        Move(); // 이동을 처리하는 메서드 호출
        FaceTarget(); // 대상을 향해 회전하는 메서드 호출
    }

    private void FaceTarget()
    {
        if (currentTarget != null)
        {
            // 대상까지의 방향 벡터를 계산하고, 캐릭터가 바르게 섰음을 보장
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            directionToTarget.y = 0f;

            // 대상을 향해 캐릭터를 회전시킴
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Move()
    {
        if (anim != null)
        {
            anim.SetBool("run", true); // "run" 애니메이션 상태를 활성화
        }

        // 대상이 있는 경우
        if (currentTarget != null)
        {
            Vector3 closestPoint = currentTarget.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            float distanceToTarget = Vector3.Distance(transform.position, closestPoint);

            // 대상이 공격 범위에 없으면 대상 쪽으로 이동
            if (distanceToTarget > attackRange)
            {
                Vector3 targetDirection = closestPoint - transform.position;
                targetDirection.y = 0f;
                targetDirection.Normalize();
                agent.isStopped = false;
                agent.SetDestination(closestPoint);
            }
            // 대상이 공격 범위에 들어왔으면 정지하고 공격
            else
            {
                if (anim != null)
                {
                    anim.SetBool("run", false); // "run" 애니메이션 상태를 비활성화
                    anim.SetBool("attack", true); // "attack" 애니메이션 상태를 활성화
                }
                agent.isStopped = true;
                if (attackCooldown <= 0f)
                {
                    Attack(); // 공격 수행
                    audioSource.Play();
                    attackCooldown = attackSpeed; // 공격 간격을 설정 (쿨다운)
                }
                // 쿨다운 업데이트
                if (attackCooldown > 0f)
                {
                    attackCooldown -= Time.deltaTime;
                }
            }
        }
        // 대상이 없으면 로컬 좌표계로 앞으로 이동
        else
        {
            agent.isStopped = false;
            if (anim != null)
            {
                anim.SetBool("run", true); // "run" 애니메이션 상태를 활성화
                anim.SetBool("attack", false); // "attack" 애니메이션 상태를 비활성화
            }

            // x 위치에 따라 가장 가까운 경로(right 또는 left)를 결정
            GameObject closestPath = transform.position.x >= 0f ? rightPath : leftPath;
            Vector3 pathPosition;

            // 팀에 따라 적절한 경로 위치를 가져옴
            if (team == Team.Blue)
            {
                pathPosition = closestPath.transform.GetChild(1).position;
            }
            else
            {
                pathPosition = closestPath.transform.GetChild(0).position;
            }

            // 목적지를 설정
            agent.SetDestination(pathPosition);
        }
    }

    public void FindTarget()
    {
        // 시야 범위 내의 모든 콜라이더를 가져옴
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange);

        // 유용한 대상들을 저장할 리스트
        List<Transform> potentialTargets = new List<Transform>();

        // 각 콜라이더에 대해 반복
        foreach (Collider collider in colliders)
        {
            // 현재 반복하는 콜라이더로부터 Unit 컴포넌트를 시도해 얻어옴
            if (collider.TryGetComponent(out Unit enemy))
            {
                // 대상의 팀이 에이전트의 팀과 다르고 에이전트가 공격 가능한 대상인 경우
                if (team != enemy.team && targets.Contains(enemy.unitType))
                {
                    // 유용한 대상으로 판단하고 리스트에 추가
                    potentialTargets.Add(collider.transform);
                }
            }
        }

        // 유용한 대상이 하나 이상 있을 때
        if (potentialTargets.Count > 0)
        {
            // 유용한 대상들 중에서 랜덤하게 대상을 선택
            int randomIndex = Random.Range(0, potentialTargets.Count);
            currentTarget = potentialTargets[randomIndex];
        }
        else
        {
            // 유용한 대상이 없으면 현재 대상을 null로 설정
            currentTarget = null;
        }
    }




    private void OnDrawGizmosSelected()
    {
        // 시각적으로 Gizmos를 그려 시야 범위와 공격 범위를 나타냄

        // 먼저, 시야 범위를 녹색으로 표현
        Gizmos.color = Color.green;

        // 현재 게임 오브젝트의 위치에서 시야 범위만큼의 반경을 가지는 원을 그립니다.
        // 이는 해당 유닛의 시야를 나타내며, 적 유닛이 시야 범위 안에 들어올 경우 감지할 수 있습니다.
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // 다음으로, 공격 범위를 빨간색으로 표현
        Gizmos.color = Color.red;

        // 현재 게임 오브젝트의 위치에서 공격 범위만큼의 반경을 가지는 원을 그립니다.
        // 이는 해당 유닛이 적 유닛을 공격할 수 있는 범위를 시각적으로 나타냅니다.
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
