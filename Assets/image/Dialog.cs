using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Scene ���� ����� ����ϱ� ���� ���ӽ����̽� �߰�

[System.Serializable]
public class Dialogue
{
    [TextArea]
    public string dialogue; // ��ȭ ������ �����ϴ� ����
    public Sprite cg; // ��ȭ �߿� ǥ���� �̹����� �����ϴ� ����
}

public class Dialog : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite_StandingCG; // ĳ���� �̹����� ǥ���ϴ� SpriteRenderer
    [SerializeField] private SpriteRenderer sprite_DialogueBox; // ��ȭ ���ڸ� ǥ���ϴ� SpriteRenderer
    [SerializeField] private Text txt_Dialogue; // ��ȭ �ؽ�Ʈ�� ǥ���ϴ� Text ������Ʈ

    private bool isDialogue = false; // ��ȭ ������ ���θ� ��Ÿ���� �÷���
    private int count = 0; // ���� ��ȭ �ε����� �����ϴ� ����

    [SerializeField] private Dialogue[] dialogue; // ��ȭ ������ ��� �ִ� �迭

    // ��ȭ ���� �޼���
    public void ShowDialogue()
    {
        ONOFF(true); // ��ȭ ����, ĳ���� �̹���, ��ȭ �ؽ�Ʈ�� ȭ�鿡 ǥ��
        count = 0; // ��ȭ �ε��� �ʱ�ȭ
        isDialogue = true; // ��ȭ �� �÷��׸� true�� ����
        NextDialogue(); // ù ��° ��ȭ�� �̵�
    }

    // ��ȭ ���� �޼���
    public void HideDialogue()
    {
        ONOFF(false); // ��ȭ ����, ĳ���� �̹���, ��ȭ �ؽ�Ʈ�� ȭ�鿡�� ����
        isDialogue = false; // ��ȭ �� �÷��׸� false�� ����

        // ��� ��ȭ�� ������ �� Scene ��ȯ
        if (count >= dialogue.Length)
        {
            // ���⿡�� ���� Scene�� �̸��� �������ּ���.
            string nextSceneName = "Scene";
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // UI ��Ҹ� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�ϴ� �޼���
    private void ONOFF(bool _flag)
    {
        sprite_DialogueBox.gameObject.SetActive(_flag);
        sprite_StandingCG.gameObject.SetActive(_flag);
        txt_Dialogue.gameObject.SetActive(_flag);
    }

    // ���� ��ȭ�� �Ѿ�� �޼���
    private void NextDialogue()
    {
        txt_Dialogue.text = dialogue[count].dialogue; // ��ȭ �ؽ�Ʈ ������Ʈ
        sprite_StandingCG.sprite = dialogue[count].cg; // ĳ���� �̹��� ������Ʈ
        count++; // ���� ��ȭ�� �̵�
    }

    void Update()
    {
        if (isDialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (count < dialogue.Length)
                {
                    NextDialogue(); // ���� ��ȭ�� �̵�
                }
                else
                {
                    HideDialogue(); // ��� ��ȭ�� ������ ��ȭ ����
                }
            }
        }
    }
}
