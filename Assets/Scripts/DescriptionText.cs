using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DescriptionText : MonoBehaviour
{
    string description = "�ֺ���224�ꡣ\n�� ����� ��������20���꣬������׼������һ���ʽ�����ʣ��ܳ���̽���ˡ�\n���������������ʹ󺣵����磬ֻ�������˿��ܼ��ĳǰ���ڽ���ļ��������ϡ����Լ���ÿ���к������붼�ǳ�Ϊһ��������Ĵ�����ǰ��������̽�����µĵ��죬Ϊ�����ṩ�������ס�����ء�\n���ң���һֱ����һ���ɻ󣬵����ǳ��������֡���ʲô��Ϊʲôû���κμ�������ǰ�����磬���е��׷�����ʲô���ѡ����ϵ�����˵������ǰ���������Ŵ��������أ����ҿƼ�����������������Ϸɵ��ؾߡ�������������������磬�Ȳ������ǲ�����ģ��ѵ����ǲ��ᱻ���ϵ�ħ�޹�����\n�ǵģ��ഫ��������Ŵ�����ħ�޳�Ѩ������ʱ����ħ������������Ϯ����ֻ����������ͨ��ħ�޲��ҹ������ϣ���Ҷ�˵���Ƕ����Ů��ıӻ���\nֹͣ˼�����㿴������ǰ��3��Ӧ����Ա��";
    public TMP_Text descriptionText;
    string[] sentences;
    int currentIndex = 0;
    bool isRunning = false;
    public GameObject[] buttons;
    bool shownChooseMemberButtons = false;
    bool startBattle = false;
    bool shownBattleButtons = false;

    void Start()
    {
        sentences = description.Split('\n');
        StartCoroutine(ShowTextOneSentenceAtATime());
    }

    private void Update()
    {
        if (!isRunning && currentIndex == sentences.Length - 1 && !shownChooseMemberButtons)
        {
            shownChooseMemberButtons = true;

            foreach (var button in buttons)
            {
                button.SetActive(true);
            }
        }
        else if (descriptionText.text.Contains("ӭս��") && !startBattle)
        {
            startBattle = true;
            BattleBGM.instance.PlayBattleBGM();
            BattleBGM.instance.battleImage.gameObject.SetActive(true);
            StartRandomEvent(2);
        }
        else if (descriptionText.text.Contains("��ѡ������¸��ж���") && !shownBattleButtons)
        {
            shownBattleButtons = true;

            // Start Battle (show atk, def, skill, item, esc buttons)
            foreach (var button in BattleBGM.instance.battleButtons)
            {
                button.SetActive(true);
            }
        }
        else if (descriptionText.text.Contains("����Ҫ��ʲô"))
        {
            // Boat Activities (Move forward, battle, fishing, repair boat)
            foreach (var button in GameManager.instance.boatActivityButtons)
            {
                button.SetActive(true);
            }
        }
        /*
        // ѡ�����֮ǰ��ֱ�ӳ��֣�����Ҫ�������
        else if (descriptionText.text.Contains("�㿪ʼ����"))
        {
            foreach (var button in GameManager.instance.buttons)
            {
                button.SetActive(true);
            }
        }
        */

        if (!isRunning && currentIndex == sentences.Length - 1)
        {
            if (descriptionText.text.Contains("�㿪ʼ����"))
            {
                foreach (var button in GameManager.instance.buttons)
                {
                    button.SetActive(true);
                }
            }
        }

        if (!isRunning && Input.GetMouseButtonDown(0))
        {
            if (descriptionText.text.Contains("��ѡ���ˣ�"))
            {
                GameManager.instance.timeText.text = "DAY 1   9am";
                GameManager.instance.locationText.text = "�밶50����";
                StartRandomEvent(0);
            }
            else if (descriptionText.text.Contains("Ϊ��"))
            {
                StartRandomEvent(1);
            }
            else if (descriptionText.text.Contains("���������1���˺�") && startBattle)
            {
                GameManager.instance.UpdateHealth(1);
                currentIndex++;
                StartCoroutine(ShowTextOneSentenceAtATime());
            }
            else if (descriptionText.text.Contains("��Ķ��ӣ�") && startBattle)
            {
                startBattle = false;
                shownBattleButtons = false;
                BattleBGM.instance.StopBattleBGM();
                GameManager.instance.timeText.text = "DAY 1   10am";
                BattleBGM.instance.battleImage.gameObject.SetActive(false);
                StartRandomEvent(4);
            }
            /*
			// ѡ��ѡ��֮����Ҫ�������
			else if (descriptionText.text.Contains("��ѡ���ˣ�"))
            {
                GameManager.instance.timeText.text = "DAY 1   7am";
                GameManager.instance.locationText.text = "�밶50����";
                StartRandomEvent(0);
            }
			*/
            else if (currentIndex < sentences.Length - 1)
            {
                currentIndex++;
                StartCoroutine(ShowTextOneSentenceAtATime());
            }
        }
        else if (isRunning && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            for (int i = 0; i <= currentIndex; i++)
            {
                descriptionText.text = sentences[i];
            }
            isRunning = false;
        }
    }

    IEnumerator ShowTextOneSentenceAtATime()
    {
        descriptionText.text = "";

        isRunning = true;
        foreach (var sentence in sentences[currentIndex])
        {
            descriptionText.text += sentence;
            yield return new WaitForSeconds(0.05f);
        }
        descriptionText.text += "\n";
        isRunning = false;
    }

    public void ChooseMember(string member)
    {
        // Playerprefs set boat member type
        //if (member == "ս����")
        //{
        //    PlayerPrefs.SetInt("BoatMember", 0);
        //}
        //else if (member == "������")
        //{
        //    PlayerPrefs.SetInt("BoatMember", 1);
        //}
        //else if (member == "������")
        //{
        //    PlayerPrefs.SetInt("BoatMember", 2);
        //}

        foreach (var button in buttons)
        {
            button.SetActive(false);
        }

        currentIndex = 0;
        description = "��ѡ���ˣ�" + member;
        sentences[0] = description;
        StartCoroutine(ShowTextOneSentenceAtATime());
    }

    // ѡ��
    public void GoOrStop(string option)
    {
        foreach (var button in GameManager.instance.buttons)
        {
            button.SetActive(false);
        }

        currentIndex = 0;
        if (option == "GO")
        {
            description = "Ϊ�˽�ʡʱ�䣬��ѡ���˼���ǰ����";
        }
        else if (option == "STOP")
        {
            description = "Ϊ�˰�ȫ�������ѡ���˷�����";
        }
        sentences[0] = description;
        StartCoroutine(ShowTextOneSentenceAtATime());
    }

    public void PlayerAttack()
    {
        foreach (var button in BattleBGM.instance.battleButtons)
        {
            button.SetActive(false);
        }

        currentIndex = 0;
        description = GameManager.instance.randomEventsDescription[3];
        sentences = description.Split(' ');
        sentences[0] = "����������" + (GameManager.instance.attack - 1) + "���˺���";
        StartCoroutine(ShowTextOneSentenceAtATime());
    }

    // ����¼�
    void StartRandomEvent(int index)
    {
        currentIndex = 0;
        description = GameManager.instance.randomEventsDescription[index];
        sentences = description.Split(' ');        
        StartCoroutine(ShowTextOneSentenceAtATime());
    }
}

