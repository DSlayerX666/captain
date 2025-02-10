using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DescriptionText : MonoBehaviour
{
    string description = "灾后历224年。\n在 神恩岛 上生活了20余年，你终于准备好了一切资金和物资，能出海探索了。\n在这个处处是迷雾和大海的世界，只有数个人口密集的城邦建立在仅存的几个大岛屿上。所以几乎每个男孩的梦想都是成为一名新世界的船长，前往迷雾海域探索到新的岛屿，为人类提供更多更居住的土地。\n而且，你一直还有一个疑惑，到底那场‘大天灾‘是什么，为什么没有任何记载天灾前的世界，还有到底发生了什么灾难。镇上的老人说过天灾前的世界有着大量的土地，而且科技发达，甚至能又在天上飞的载具。你很难想象那样的世界，先不提这是不是真的，难道他们不会被天上的魔兽攻击吗？\n是的，相传迷雾深处有着大量的魔兽巢穴，所以时常有魔兽来到海面上袭击船只。但好在普通的魔兽不敢攻击岸上，大家都说这是多亏了女神的庇护。\n停止思索，你看向了眼前的3个应征船员：";
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
        else if (descriptionText.text.Contains("迎战！") && !startBattle)
        {
            startBattle = true;
            BattleBGM.instance.PlayBattleBGM();
            BattleBGM.instance.battleImage.gameObject.SetActive(true);
            StartRandomEvent(2);
        }
        else if (descriptionText.text.Contains("请选择你的下个行动：") && !shownBattleButtons)
        {
            shownBattleButtons = true;

            // Start Battle (show atk, def, skill, item, esc buttons)
            foreach (var button in BattleBGM.instance.battleButtons)
            {
                button.SetActive(true);
            }
        }
        else if (descriptionText.text.Contains("现在要干什么"))
        {
            // Boat Activities (Move forward, battle, fishing, repair boat)
            foreach (var button in GameManager.instance.boatActivityButtons)
            {
                button.SetActive(true);
            }
        }
        /*
        // 选项出现之前（直接出现，不需要按左键）
        else if (descriptionText.text.Contains("你开始警惕"))
        {
            foreach (var button in GameManager.instance.buttons)
            {
                button.SetActive(true);
            }
        }
        */

        if (!isRunning && currentIndex == sentences.Length - 1)
        {
            if (descriptionText.text.Contains("你开始警惕"))
            {
                foreach (var button in GameManager.instance.buttons)
                {
                    button.SetActive(true);
                }
            }
        }

        if (!isRunning && Input.GetMouseButtonDown(0))
        {
            if (descriptionText.text.Contains("你选择了："))
            {
                GameManager.instance.timeText.text = "DAY 1   9am";
                GameManager.instance.locationText.text = "离岸50海里";
                StartRandomEvent(0);
            }
            else if (descriptionText.text.Contains("为了"))
            {
                StartRandomEvent(1);
            }
            else if (descriptionText.text.Contains("对你造成了1点伤害") && startBattle)
            {
                GameManager.instance.UpdateHealth(1);
                currentIndex++;
                StartCoroutine(ShowTextOneSentenceAtATime());
            }
            else if (descriptionText.text.Contains("落荒而逃！") && startBattle)
            {
                startBattle = false;
                shownBattleButtons = false;
                BattleBGM.instance.StopBattleBGM();
                GameManager.instance.timeText.text = "DAY 1   10am";
                BattleBGM.instance.battleImage.gameObject.SetActive(false);
                StartRandomEvent(4);
            }
            /*
			// 选了选项之后（需要按左键）
			else if (descriptionText.text.Contains("你选择了："))
            {
                GameManager.instance.timeText.text = "DAY 1   7am";
                GameManager.instance.locationText.text = "离岸50海里";
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
        //if (member == "战斗型")
        //{
        //    PlayerPrefs.SetInt("BoatMember", 0);
        //}
        //else if (member == "后勤型")
        //{
        //    PlayerPrefs.SetInt("BoatMember", 1);
        //}
        //else if (member == "工匠型")
        //{
        //    PlayerPrefs.SetInt("BoatMember", 2);
        //}

        foreach (var button in buttons)
        {
            button.SetActive(false);
        }

        currentIndex = 0;
        description = "你选择了：" + member;
        sentences[0] = description;
        StartCoroutine(ShowTextOneSentenceAtATime());
    }

    // 选项
    public void GoOrStop(string option)
    {
        foreach (var button in GameManager.instance.buttons)
        {
            button.SetActive(false);
        }

        currentIndex = 0;
        if (option == "GO")
        {
            description = "为了节省时间，你选择了继续前进。";
        }
        else if (option == "STOP")
        {
            description = "为了安全起见，你选择了返航。";
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
        sentences[0] = "你对鱼人造成" + (GameManager.instance.attack - 1) + "点伤害！";
        StartCoroutine(ShowTextOneSentenceAtATime());
    }

    // 随机事件
    void StartRandomEvent(int index)
    {
        currentIndex = 0;
        description = GameManager.instance.randomEventsDescription[index];
        sentences = description.Split(' ');        
        StartCoroutine(ShowTextOneSentenceAtATime());
    }
}

