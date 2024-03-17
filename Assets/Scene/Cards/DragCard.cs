using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // �巡�� �߿� ������ ������
    [SerializeField] GameObject PrefabToInstantiate;

    // �巡�׵Ǵ� UI ����� RectTransform
    [SerializeField] RectTransform UIDragElement;

    // Canvas�� RectTransform (�巡�� ������ �����ϱ� ����)
    [SerializeField] RectTransform Canvas;

    // �巡�� ���� ������ ���� ������ ��ġ
    private Vector2 mOriginalLocalPointerPosition;

    // �巡�� ���� ������ �г��� ���� ��ġ
    private Vector3 mOriginalPanelLocalPosition;

    // UIDragElement�� �ʱ� ��ġ
    private Vector2 mOriginalPosition;

    // �÷��̾��� ���� ����
    private PlayerMana playerMana;

    // ī���� ������ (���� ��� ��)
    private CardData cardData;

    void Start()
    {
        // UIDragElement�� �ʱ� ��ġ�� ����
        mOriginalPosition = UIDragElement.localPosition;

        // PlayerMana ��ũ��Ʈ�� ã�Ƽ� �Ҵ�
        playerMana = FindObjectOfType<PlayerMana>();

        // ī�� ������ ��ũ��Ʈ�� ã�Ƽ� �Ҵ�
        cardData = GetComponent<CardData>();
    }

    // �巡�� ���� �� ȣ��
    public void OnBeginDrag(PointerEventData data)
    {
        // UIDragElement�� ���� ���� ��ġ ����
        mOriginalPanelLocalPosition = UIDragElement.localPosition;

        // ���� ���콺 ��ġ�� Canvas�� ���� ��ǥ�� ��ȯ�Ͽ� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;

        // ���� ���콺 ��ġ�� Canvas�� ���� ��ǥ�� ��ȯ
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out localPointerPosition))
        {
            // �巡�� ���� ���� ���콺 ��ġ�� ���� ���콺 ��ġ ���� ���� ���
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;

            // UIDragElement�� ��ġ�� ������Ʈ�Ͽ� �巡�� ȿ�� ����
            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }

    // �巡�� ���� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        // UIDragElement�� �ʱ� ��ġ�� �ε巴�� �̵���Ű�� �ڷ�ƾ ����
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));

        // ī���� ���� ����� ������
        float manaCost = cardData.manaCost;

        // ���� ������ ī���� ���� ��뺸�� ũ�ų� ������ ����
        if (playerMana.GetCurrentMana() >= manaCost)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ���콺 ��ġ���� ���̸� ���� �浹 Ȯ��
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                // ���� ���̽��� ī�带 ���� �� ������ ���� ó��
                if (hit.collider.CompareTag("EnemyBase"))
                {
                    Debug.Log("You can't throw the card to ENEMYBASE!");
                    // ������ ��������� ���� ���̽��� ī�带 �������� �õ��ϸ� ���
                    return;
                }

                // ���� ���̽��� ī�带 ������ ������ ī�带 ���
                playerMana.ReduceMana(manaCost);
                Vector3 worldPoint = hit.point;
                CreateObject(worldPoint);
            }
        }
        else
        {
            // UIDragElement�� �ʱ� ��ġ�� �ε巴�� �̵���Ű�� �ڷ�ƾ ����
            StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));
        }
    }

    // UI ��Ҹ� �ε巴�� �̵���Ű�� �ڷ�ƾ
    public IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startingPos = r.localPosition;

        // ���� �ð� ���� �ε巴�� �̵�
        while (elapsedTime < duration)
        {
            r.localPosition = Vector2.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // �̵� �Ϸ� �� ���� ��ġ ����
        r.localPosition = targetPosition;
    }

    // �������� �־��� ��ġ�� ����
    public void CreateObject(Vector3 position)
    {
        GameObject obj = Instantiate(PrefabToInstantiate, position + Vector3.up * 0.5f, Quaternion.identity);
    }
}
