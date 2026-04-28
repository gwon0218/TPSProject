using System.Collections;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    PlayerMove player;


    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }


    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    public GameState gState;

    public GameObject gameLabel;
    TMP_Text gameText;

    void Start()
    {
        gState = GameState.Ready;

        gameText = gameLabel.GetComponent<TMP_Text>();
        gameText.text = "Ready...";
        gameText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();

    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "Go!";

        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);

        gState = GameState.Run;

    
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hp < 0)
        {
            gState = GameState.GameOver;
            gameLabel.SetActive(true);
            gameText.text = "Game Over!";
            gameText.color = new Color32(255, 0, 0, 255);

            gState = GameState.GameOver;
        }
    }
}
