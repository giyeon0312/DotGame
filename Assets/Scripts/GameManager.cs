using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int healthPoint;

    public void NextStage()
    {
        stageIndex++;
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Lose HP
            healthPoint--;

            //Player Reposition
            collision.attachedRigidbody.velocity = Vector2.zero;//낙하 속도를 0으로
            collision.transform.position = new Vector3(0, 0, -1);
        }
    }
}
