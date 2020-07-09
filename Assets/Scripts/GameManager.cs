using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public GameObject[] Stages;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int healthPoint;
    public PlayerController player;

    float watchElapsed = 0;

    //UI
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIWatch;
    public GameObject Btn_restart;
    public GameObject Btn_save;

    public void Start()
    {
        SoundManager.instance.PlayBGM("BGM1");
    }

    public void Update()
    {
        UIPoint.text = (totalPoint+stagePoint).ToString();     
        watchElapsed += UnityEngine.Time.deltaTime;
        UIWatch.text = watchElapsed.ToString("0.00");
        
    }

    public void NextStage()
    {
        //StageClear Sound
        SoundManager.instance.PlaySE("AudioStageClear");

        //Change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
            SoundManager.instance.PlayBGM("BGM"+(stageIndex + 1).ToString());

            //ResetStopwatch
            ResetWacth();

        }//Game Clear
        else
        {
            //Player Control Lock
            Time.timeScale = 0;
            //Result UI
            Debug.Log("게임클리어");
            Btn_restart.SetActive(true);
            Btn_save.SetActive(true);
        }

        totalPoint += stagePoint;
        stagePoint = 0;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Lose HP
            HealthDown();

            //Player Reposition
            if (healthPoint >= 1)
            {
                collision.attachedRigidbody.velocity = Vector2.zero;//낙하 속도를 0으로
                collision.transform.position = new Vector3(0, 0, -1);
                //PlayerReposition();
            }
  
        }
    }

    public void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        //player.VelocityZero();
    }

    public void HealthDown()
    {
        if (healthPoint > 1)
        {
            healthPoint--;
            UIhealth[healthPoint].color = new Color(1, 1, 1, 0.2f);
        }
        else
        {
            //All healthUI Off
            UIhealth[0].color = new Color(1, 1, 1, 0.2f);

            //Plyaer Die Effect
            player.OnDie();

            //Result UI
            Debug.Log("죽었다..");

            //Retry Button UI
            Btn_restart.SetActive(true);
        }
    }

    public void ResetWacth()
    {
        watchElapsed= 0;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Save()
    {
        //후에 구현할 것
        Debug.Log("남은 healthPoint,totalPoint,time으로 점수 랭킹 구현하기");
    }
}
