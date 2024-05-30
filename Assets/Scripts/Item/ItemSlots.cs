using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    public ItemData item;

    public GameObject baseSlot;
    public GameObject emptySlot;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public Image quantityImg;
    public TextMeshProUGUI quatityText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        baseSlot.SetActive(false);
        emptySlot.SetActive(true);
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        emptySlot.SetActive(false);
        baseSlot.SetActive(true);

        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        if(quantity > 1)
        {
            quatityText.text = quantity.ToString();
        }
        else
        {
            quantityImg.enabled = false;
        }
        
        if(outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        emptySlot.SetActive(true);
        baseSlot.SetActive(false);

        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
