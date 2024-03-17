using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    List<Transform> targets = new List<Transform>();  // 여러 개의 타겟을 관리하는 List
    int currentTargetIndex = 0;  // 현재 선택된 타겟의 인덱스
    bool isMoving = false;  // 이동 중인지 여부를 나타내는 플래그

    private void Awake()
    {
        // NavMeshAgent 컴포넌트 가져오기
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 마우스 왼쪽 버튼이 클릭되면
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            // 모든 목표 지점을 순차적으로 이동하는 메서드 호출
            StartCoroutine(MoveToTargets());
        }
    }

    IEnumerator MoveToTargets()
    {
        // 이동 중 플래그 설정
        isMoving = true;

        // 모든 목표 지점을 순차적으로 이동
        for (int i = 0; i < targets.Count; i++)
        {
            Transform currentTarget = targets[i];

            if (currentTarget != null)
            {
                // NavMeshAgent를 사용하여 현재 타겟의 위치로 이동
                agent.SetDestination(currentTarget.position);

                // 이동이 완료될 때까지 대기
                yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            }
        }

        // 이동 완료 후 이동 중 플래그 해제
        isMoving = false;
    }
}
