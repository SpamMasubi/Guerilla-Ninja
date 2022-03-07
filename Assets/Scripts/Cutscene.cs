using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public GameObject[] cutsceneImage;
    public Text[] cutsceneTexts;

    void Start()
    {
        switch (ChapterIntro.chapters)
        {
            case 3:
                cutsceneImage[0].SetActive(false);
                cutsceneImage[1].SetActive(true);
                cutsceneTexts[0].text = "The Guerilla Ninja finds out that the enemies " +
                "are known as the North Star Army. They are in Vietnam for their main reason.";
                cutsceneTexts[1].text = "The North Star wants to start WWIII with both the Soviet Union " +
                "and the United States. By having their soldiers infiltrate both side, the two will blame each other.";
                cutsceneTexts[2].text = "However, a darker truth of the North Star Army will bring out " +
                "the Guerilla Ninja's rage and the true form of Ninjutsu.";
                break;
            case 4:
                cutsceneImage[1].SetActive(false);
                cutsceneImage[2].SetActive(true);
                cutsceneTexts[0].text = "As the Guerilla Ninja fights to their main HQ, he gets closer " +
                "and closer to the truth. With their aid, North Vietnam will succeed in their reunification with South Vietnam.";
                cutsceneTexts[1].text = "However, the North Star Army outnumbers our ninja soldier and is captured " +
                "by the enemy. Placed in a prison cell, the Guerilla Ninja devised a plan and makes his escape.";
                cutsceneTexts[2].text = "Our ninja soldier finds a dossier regardings the North Star Army's plan, but " +
                "one document has him changing his vengeful goals to something else.";
                break;
            case 5:
                cutsceneImage[2].SetActive(false);
                cutsceneImage[3].SetActive(true);
                cutsceneTexts[0].text = "After reading the dossier and documents, the Guerilla Ninja learns that the North Star Army were" +
                "the one that ambushed his platoon and killed his ninja master. A truth haunting him forever.";
                cutsceneTexts[1].text = "He felt that he had finally avenge his fallen brethren and master, yet he isn't at peace with it." +
                "The war may be over, but a new threat has risen. As Saigon fell, he escaped the country.";
                cutsceneTexts[2].text = "The dossier was in UN's custody and made the North Star Army their #1 concern. With new threats " +
                "looms over the Cold War, the Guerilla Ninja continues to train his Ninjutsu while coping with his nightmare.";
                break;
            default:
                break;
        }
    }
}
