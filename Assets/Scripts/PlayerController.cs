using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float accelerate = 3f;
	[SerializeField] private float sensitivity = 100f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float clamp = 89f;
    [SerializeField] private float gravityScale = -9.81f;
    [SerializeField] private float jumpPower = 5f;

	//private Vector3 dir;

	private Vector3 dirH;
	private Vector3 dirV;

    private CharacterController character;
	private float moveSpeedH;
    private float moveSpeedV;
    private float mouseX;
    private float mouseY;
    private float gravity;
    private bool isCrouching;
	private void Start()
    {
        character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Jump();
		Move();
        Crouch();
		Facing();
	}

    private void Move()
    {
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		if (horizontal != 0)
        {
			dirH = (transform.right * horizontal);
			moveSpeedH += speed * Time.deltaTime * accelerate;
            if(moveSpeedH > speed)
                moveSpeedH = speed;
        }
		else
		{
			moveSpeedH -= speed * Time.deltaTime * accelerate;
			if (moveSpeedH < 0)
				moveSpeedH = 0;
		}

		if (vertical != 0)
        {
			dirV = (transform.forward * vertical);
			moveSpeedV += speed * Time.deltaTime * accelerate;
			if (moveSpeedV > speed)
				moveSpeedV = speed;
		}
        else
        {
			moveSpeedV -= speed * Time.deltaTime * accelerate;
			if (moveSpeedV < 0)
				moveSpeedV = 0;
		}

        Gravity();
		character.Move(( ((transform.right * moveSpeedH) + (transform.forward * moveSpeedV)) + (Vector3.up * gravity)) * Time.deltaTime);
    }

    private void Gravity()
    {
        if(!character.isGrounded)
            gravity += Time.deltaTime * gravityScale;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && character.isGrounded)
        {
            gravity = jumpPower;
        }
    }

    private void Crouch()
    {
		isCrouching = Input.GetKey(KeyCode.LeftControl);

		// Crouch
		if (isCrouching)
		{
			character.height = Mathf.Lerp(character.height, 1, Time.deltaTime * 8f);
		}
		else
		{
			float lastHeight = character.height;
			character.height = Mathf.Lerp(character.height, 2, Time.deltaTime * 8f);
			Vector3 tmpPosition = transform.position;
			//Debug.Log((character.height - lastHeight));
			tmpPosition.y += (character.height - lastHeight);
			transform.position = tmpPosition;
		}
	}


	private void Facing()
    {
		mouseX += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
		mouseY -= Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        mouseY = Mathf.Clamp(mouseY, -clamp, clamp);

		Quaternion rotateX = Quaternion.Euler(mouseY, 0, 0);
		Quaternion rotateY = Quaternion.Euler(0, mouseX, 0);

        transform.rotation = rotateY;
        camera.transform.localRotation = rotateX;
	}
}
