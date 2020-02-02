using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()//1초에 60프레임,단발적인 키 입력
    {
        Walking();          //IsWalking Animation
        DirectionFlip();
        Jump();
        StopSpeed();
    }
       
    void FixedUpdate()//보통 1초에 50프레임,계속적인 키 입력
    {
        Move();           //Move speed
        Landing();       //Landing Platform
    }

    private void Move()
    {
        //move speed 키보드 입력으로 움직임
        float h = Input.GetAxisRaw("Horizontal");//Horizontal입력이 있는 축 float값 반환
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //max speed x축의 속도
        if (rigid.velocity.x > maxSpeed)//right maxSpeed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))//left maxSpeed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

    }

    private void StopSpeed()
    {
        //키보드를 떼면 속력을 줄이도록
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2((rigid.velocity.normalized.x) * 0.5f, rigid.velocity.y);
            //rigid.velocity.x는 벡터로 방향과크기 둘 다 가지므로 
            //normalized로 방향은 유지하고 벡터크기를 1로 만든 상태(단위벡터)
        }
    }

    private void DirectionFlip()
    {
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = (Input.GetAxisRaw("Horizontal") == -1);//bool값 return하도록
    }

    private void Walking()
    {
        //animation전환
        //횡이동속도(벡터값이므로 절댓값으로)가 0이면 false
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("IsJumping", true);
            //이후IsGround에서 애니메이션 멈춤
        }
          
    }

    private void Landing()
    {
        //player가 아래로 내려갈때만(y축 속력이 음수일때)raycast를 쏘자
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));//충돌한 오브젝트의 콜라이더 정보 저장
                                                                                                                    //GetMask():레이어 이름에 해당하는 정수값 리턴여기선 8

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("IsJumping", false);
                Debug.Log(rayHit.collider.name);
            }
        }   
    }
}
