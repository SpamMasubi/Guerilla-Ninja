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
                "are known as the North Star Army. They are the in Vietnam for their main reason.";
                cutsceneTexts[1].text = "The North Star wants to start WWIII with both the Soviet Union " +
                "and the United States. By having their soldiers infiltrate both side, the two will blame each other.";
                cutsceneTexts[2].text = "However, a darker truth of the North Star Army will bring out " +
                "the Guerilla Ninja's rage and the true form of Ninjutsu.";
                break;
            case 4:
                break;
            default:
                break;
        }
    }
}
