using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int nextMove;
    private Rigidbody2D rigid;
    CapsuleCollider2D enemyCollider;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        Invoke("RandomMove", 5);
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 

    void FixedUpdate()
    {   
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);//오브젝트의 앞 위치벡터
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHitPlatform = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));//충돌한 오브젝트의 콜라이더 정보 저장                                                                                   //GetMask():레이어 이름에 해당하는 정수값 리턴여기선 8

        
        if (rayHitPlatform.collider == null)//오브젝트의 앞에 platform이 없으면
        {                           //방향을 바꾸고 지금 움직임을 바꾼다
            Turn();
        }

        //TryAttack
        Debug.DrawRay(frontVec, Vector3.left, new Color(1, 0, 0));
        RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, Vector3.left, 1, LayerMask.GetMask("Player"));
        if (rayHitPlayer.collider != null)
        {
            //사실 여기 TryAttack()을 두고 Attack()은 oncollision에 두어야할거 같다
            Attack();
        }
        
        
    }
    
    //재귀함수
    void RandomMove()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Flip Sprite
        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        //Recursive
        Invoke("RandomMove", 5);
    }

    void Turn()
    {
        nextMove = nextMove * (-1);
        spriteRenderer.flipX = (nextMove == 1);
        CancelInvoke();
        Invoke("RandomMove", 5);
    }
    void Attack()
    {
        anim.SetTrigger("IsAttack");
    }

    public void OnDamaged()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        enemyCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Destroy
        Invoke("Deactivate", 5);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
