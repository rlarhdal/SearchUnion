using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;

    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.player.controller;
    }

    public void UseFlashlight(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            Debug.Log("F ´©¸§");
            curEquip.UseFlashlight();
        }
    }

}
