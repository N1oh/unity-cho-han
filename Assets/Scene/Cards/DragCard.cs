using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 드래그 중에 생성될 프리팹
    [SerializeField] GameObject PrefabToInstantiate;

    // 드래그되는 UI 요소의 RectTransform
    [SerializeField] RectTransform UIDragElement;

    // Canvas의 RectTransform (드래그 영역을 제한하기 위함)
    [SerializeField] RectTransform Canvas;

    // 드래그 시작 시점의 로컬 포인터 위치
    private Vector2 mOriginalLocalPointerPosition;

    // 드래그 시작 시점의 패널의 로컬 위치
    private Vector3 mOriginalPanelLocalPosition;

    // UIDragElement의 초기 위치
    private Vector2 mOriginalPosition;

    // 플레이어의 마나 관리
    private PlayerMana playerMana;

    // 카드의 데이터 (마나 비용 등)
    private CardData cardData;

    void Start()
    {
        // UIDragElement의 초기 위치를 저장
        mOriginalPosition = UIDragElement.localPosition;

        // PlayerMana 스크립트를 찾아서 할당
        playerMana = FindObjectOfType<PlayerMana>();

        // 카드 데이터 스크립트를 찾아서 할당
        cardData = GetComponent<CardData>();
    }

    // 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData data)
    {
        // UIDragElement의 현재 로컬 위치 저장
        mOriginalPanelLocalPosition = UIDragElement.localPosition;

        // 현재 마우스 위치를 Canvas의 로컬 좌표로 변환하여 저장
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;

        // 현재 마우스 위치를 Canvas의 로컬 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out localPointerPosition))
        {
            // 드래그 시작 시의 마우스 위치와 현재 마우스 위치 간의 차이 계산
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;

            // UIDragElement의 위치를 업데이트하여 드래그 효과 적용
            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }

    // 드래그 종료 시 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        // UIDragElement를 초기 위치로 부드럽게 이동시키는 코루틴 시작
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));

        // 카드의 마나 비용을 가져옴
        float manaCost = cardData.manaCost;

        // 현재 마나가 카드의 마나 비용보다 크거나 같으면 실행
        if (playerMana.GetCurrentMana() >= manaCost)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 마우스 위치에서 레이를 쏴서 충돌 확인
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                // 적의 베이스에 카드를 던질 수 없도록 예외 처리
                if (hit.collider.CompareTag("EnemyBase"))
                {
                    Debug.Log("You can't throw the card to ENEMYBASE!");
                    // 마나는 충분하지만 적의 베이스에 카드를 던지려고 시도하면 취소
                    return;
                }

                // 적의 베이스에 카드를 던지지 않으면 카드를 사용
                playerMana.ReduceMana(manaCost);
                Vector3 worldPoint = hit.point;
                CreateObject(worldPoint);
            }
        }
        else
        {
            // UIDragElement를 초기 위치로 부드럽게 이동시키는 코루틴 시작
            StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));
        }
    }

    // UI 요소를 부드럽게 이동시키는 코루틴
    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;

        // 일정 시간 동안 부드럽게 이동
        while (elapsedTime < duration)
        {
            r.localPosition = Vector2.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // 이동 완료 후 최종 위치 설정
        r.localPosition = targetPosition;
    }

    // 프리팹을 주어진 위치에 생성
    public void CreateObject(Vector3 position)
    {
        GameObject obj = Instantiate(PrefabToInstantiate, position + Vector3.up * 0.5f, Quaternion.identity);
    }
}
