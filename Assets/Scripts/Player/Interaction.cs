using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public Image crossHair;
    public Sprite[] crosshairs;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastCheckTime > checkRate) 
        {
            lastCheckTime = Time.time;

            // 화면 정중앙에 ray 쏘기
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetCrossHairChange();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                // 크로스헤어 변경
                crossHair.sprite = crosshairs[0];
                crossHair.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    private void SetCrossHairChange()
    {
        crossHair.sprite = crosshairs[1];
        crossHair.transform.localScale = new Vector3(3f, 4f, 3f);
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
        }
    }
}
