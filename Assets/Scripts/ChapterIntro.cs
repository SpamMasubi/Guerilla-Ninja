using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterIntro : MonoBehaviour
{
    public GameObject startGame;
    public Text chapterIntro;
    public Text chapterText;
    private bool canStartGame;
    private AudioSource selection;
    public static int chapters = 1;

    // Start is called before the first frame update
    void Start()
    {

        FindObjectOfType<GameManager>().health = 100;
        selection = GetComponent<AudioSource>();
        switch (chapters)
        {
            case 1:
                chapterIntro.text = "Mission 1";
                chapterText.text = "Soldier of Ninjutsu";
                break;
            case 2:
                chapterIntro.text = "Mission 2";
                chapterText.text = "No Ordinary War";
                break;
            case 3:
                chapterIntro.text = "Mission 3";
                chapterText.text = "Beginning of the End";
                break;
            case 4:
                chapterIntro.text = "Final Mission";
                chapterText.text = "The Truth";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(canStart());
        if(Input.GetButtonDown("Submit") && canStartGame)
        {
            selection.Play();
            canStartGame = false;
            Invoke("LoadScene", 1f);
        }
    }

    private IEnumerator canStart()
    {
        yield return new WaitForSeconds(6);
        startGame.SetActive(true);
        canStartGame = true;
    }

    private void LoadScene()
    {
        switch (chapters)
        {
            case 1:   
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(4);
                break;
            case 3:
                SceneManager.LoadScene(6);
                break;
            case 4:
                SceneManager.LoadScene(8);
                break;
            default:
                break;
        }
        
    }
}
