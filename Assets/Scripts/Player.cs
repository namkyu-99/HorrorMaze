using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigidbody;
    Animator animator;

    float MovementSpeed;
    float WalkingSpeed = 7f;
    float RunningSpeed = 11f;
    float JumpPower = 13f;

    bool isMoving;

    bool isForwardWall;
    bool isBackWall;
    bool isLeftWall;
    bool isRightWall;
    bool isGround;

    float RotationSpeed = 200f;
    float xRotate;

    public GameObject Bullet;
    public Transform spawnBullet;
    float bulletPower = 1000f;

    public GameObject Marker;

    public AudioSource move;
    public AudioClip Walk;
    public AudioClip Run;
    public AudioSource Click;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        xRotate = Camera.main.transform.localEulerAngles.x;
    }

    void Update()
    {
        if(GameManager.instance.GameEnd == 0)
        {
            Move();
            Fire();
        }
        else
            move.Stop();
        StopToWall();
        Rotate();
        Marker.transform.position = new Vector3(this.transform.position.x, Marker.transform.position.y, this.transform.position.z);
    }

    void Move()
    {
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        
        if(isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MovementSpeed = RunningSpeed;
                move.clip = Run;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
            else
            {
                MovementSpeed = WalkingSpeed;
                move.clip = Walk;
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }

            if(!move.isPlaying)
                move.Play();
        }
        else
        {
            if(move.isPlaying)
                move.Stop();
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        if (!isGround && move.isPlaying)
        {
            move.Stop();
        }

        if (Input.GetKey(KeyCode.W) && !isForwardWall)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.S) && !isBackWall)
        {
            transform.Translate(Vector3.back * Time.deltaTime * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.A) && !isLeftWall)
        {
            transform.Translate(Vector3.left * Time.deltaTime * MovementSpeed);
        }
        if (Input.GetKey(KeyCode.D) && !isRightWall)
        {
            transform.Translate(Vector3.right * Time.deltaTime * MovementSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rigidbody.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            isGround = false;
        }
    }

    void StopToWall()     // 벽 뚫기 방지를 위한 레이캐스트 충돌 검사
    {
        Vector3 Offset = new Vector3(0, 0.5f, 0);
        Debug.DrawRay(transform.position + Offset, transform.forward * 0.5f, Color.red);
        Debug.DrawRay(transform.position + Offset, -transform.forward * 0.5f, Color.red);
        Debug.DrawRay(transform.position + Offset, -transform.right * 0.5f, Color.red);
        Debug.DrawRay(transform.position + Offset, transform.right * 0.5f, Color.red);
        Debug.DrawRay(transform.position + Offset, -transform.up * 0.7f, Color.red);

        int layerMask = (-1) - (1 << LayerMask.NameToLayer("NotRay"));
        isForwardWall = Physics.Raycast(transform.position + Offset, transform.forward, 0.5f, layerMask);
        isBackWall = Physics.Raycast(transform.position + Offset, -transform.forward, 0.5f, layerMask);
        isLeftWall = Physics.Raycast(transform.position + Offset, -transform.right, 0.5f, layerMask);
        isRightWall = Physics.Raycast(transform.position + Offset, transform.right, 0.5f, layerMask);
        isGround = Physics.Raycast(transform.position + Offset, -transform.up, 0.7f, layerMask);
    }

    void Rotate()
    {
        float yRotation = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSpeed;
        float yRotate = transform.eulerAngles.y + yRotation;
        float xRotation = -Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotation, -45, 70);

        transform.rotation = Quaternion.Euler(new Vector3(0, yRotate, 0));
        spawnBullet.rotation = Quaternion.Euler(new Vector3(0, yRotate, 0));
        Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(xRotate, 0, 0));
        spawnBullet.transform.localRotation = Quaternion.Euler(new Vector3(xRotate, 0, 0));
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(1) && GameManager.instance.knife > 0)
        {
            GameObject bulletInstance = Instantiate(Bullet, spawnBullet.position, spawnBullet.rotation);
            Physics.IgnoreCollision(GetComponent<Collider>(), bulletInstance.GetComponent<Collider>());
            Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(bulletRigidbody.transform.forward * bulletPower);
            GameManager.instance.knife--;
            animator.SetTrigger("Throw");
        }
        else if(Input.GetMouseButtonDown(1))
            Click.Play();
    }
}