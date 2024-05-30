using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private float camCurXRot; // ī�޶� ȸ��
    public float lookSensitivity; // ī�޶� ����
    private Vector2 mouseDelta; // input ������ �޾ƿ��� ���콺 delta ��

    [HideInInspector]
    public bool canLook = true;
    [HideInInspector]
    public bool isRun = false;
    [HideInInspector]
    public bool isCrouch = false;

    public Action inventory;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        speed = moveSpeed;
        originPosY = transform.position.y;
        cam = Camera.main;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
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
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context) // inputsystem���� move ���� �� �� �޾ƿ���
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLookInput(InputAction.CallbackContext context) // ���콺 delta �� �޾ƿ���
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
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
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void CameraLook() // ī�޶� ������ ����
    {
        camCurXRot += mouseDelta.y * lookSensitivity; // ���� ���ؼ� ������
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // camcurxrot�� min���� max���� �ε巴��
        cam.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        flashlight.transform.localEulerAngles = new Vector3(0, -90, camCurXRot - 90);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void Move() // ������ ����
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

    private bool IsGrounded() // �÷��̾ ������ �ִ��� ray�� Ȯ��
    {
        Ray[] rays = new Ray[4] //���̸� ��� ���� �װ� ������ ����
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
