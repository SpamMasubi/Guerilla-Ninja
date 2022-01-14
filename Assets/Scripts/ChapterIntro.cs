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

    // Start is called before the first frame update
    void Start()
    {
        selection = GetComponent<AudioSource>();
        chapterIntro.text = "Mission 1";
        chapterText.text = "Soldier of Ninjutsu";
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(canStart());
        if(Input.GetKeyDown(KeyCode.Return) && canStartGame)
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
        SceneManager.LoadScene(2);
    }
}
