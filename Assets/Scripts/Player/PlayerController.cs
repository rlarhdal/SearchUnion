using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("# Movement")]
    public float speed;
    public float moveSpeed;
    public float runSpeed;
    public float crouchSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;
    private float originPosY;
    private float applyCrouchPosY;
    public float crouchPosY;
    public float useStamina;
    public GameObject flashlight;

    [Header("# Look")]
    public Transform cameraContainer;
    public Camera cam;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot; // 카메라 회전
    public float lookSensitivity; // 카메라 감도
    private Vector2 mouseDelta; // input 값으로 받아오는 마우스 delta 값

    [Header("# UI")]
    public GameObject intro;
    public GameObject outTro;
    public TextMeshProUGUI resultText;

    [HideInInspector]
    public bool canLook = true;
    [HideInInspector]
    public bool isRun = false;
    [HideInInspector]
    public bool isCrouch = false;
    [HideInInspector]
    public bool playing = false;

    //private Animator animator;

    public Action inventory;
    public Action move;
    public Action run;
    public Action jump;


    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        speed = moveSpeed;
        originPosY = transform.position.y;
        cam = Camera.main;
        //animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if(canLook)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            if(CharacterManager.Instance.player.conditions.isDead == true)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 0.2f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRun)
        {
            if (CharacterManager.Instance.player.conditions.UseStamina(useStamina))
            {
                run?.Invoke();
                //animator.SetBool("IsRunning", true);
                speed = runSpeed;
            }
            else
            {
                speed = moveSpeed;
            }
        }
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnStart()
    {
        intro.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void Result()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnMoveInput(InputAction.CallbackContext context) // inputsystem에서 move 했을 때 값 받아오기
    {
        if(context.phase == InputActionPhase.Performed)
        {
            move?.Invoke();
            //animator.SetBool("IsWalking", true);
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLookInput(InputAction.CallbackContext context) // 마우스 delta 값 받아오기
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            jump?.Invoke();
            //animator.SetTrigger("Jump");
            rigid.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            isRun = true;
            speed = runSpeed;
        }
        else
        {
            isRun = false;
            speed = moveSpeed;
        }
    }

    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && IsGrounded())
        {
            isCrouch = true;
            speed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            isCrouch = false;
            speed = moveSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        Time.timeScale = 0;
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void CameraLook() // 카메라 움직임 로직
    {
        camCurXRot += mouseDelta.y * lookSensitivity; // 감도 곱해서 느리게
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // camcurxrot를 min에서 max까지 부드럽게
        cam.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        flashlight.transform.localEulerAngles = new Vector3(0, -90, camCurXRot - 90);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void Move() // 움직임 로직
    {   
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= speed;
        dir.y = rigid.velocity.y;

        rigid.velocity = dir;
    }

    private void Run()
    {
        if (isRun)
        {
            if (CharacterManager.Instance.player.conditions.UseStamina(useStamina))
            {
                speed = runSpeed;
            }
            else
            {
                speed = moveSpeed;
            }
        }
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = cameraContainer.transform.localPosition.y;
        int cnt = 0;

        while(_posY != applyCrouchPosY)
        {
            cnt++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.2f);
            cameraContainer.transform.localPosition = new Vector3(0, _posY, 0);
            if (cnt > 15) break;
            yield return null;
        }
        cameraContainer.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    private bool IsGrounded() // 플레이어가 땅위에 있는지 ray로 확인
    {
        Ray[] rays = new Ray[4] //레이를 찍기 위해 네개 레이저 만듦
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
