using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlots[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("# Selected Item")]
    private ItemSlots selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDesc;
    public GameObject useButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerConditions condition;

    void Start()
    {
        controller = CharacterManager.Instance.player.controller;
        condition = CharacterManager.Instance.player.conditions;

        controller.inventory += Toggle;
        CharacterManager.Instance.player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlots[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlots>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.player.itemData;

        if(data.canStack)
        {
            ItemSlots slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.player.itemData = null;
                return;
            }
        }

        ItemSlots emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.player.itemData = null;
            return;
        }

        CharacterManager.Instance.player.itemData = null;
    }

    private void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    private ItemSlots GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    private ItemSlots GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDesc.text = selectedItem.item.description;

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDesc.text = string.Empty;

        useButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if(selectedItem.item.type == ItemType.Consumable)
        {
            for(int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch(selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        condition.Adrenaline(selectedItem.item.consumables[i].value);
                        break;
                }
            }
            RemoveSeletedItem();
        }
    }

    private void RemoveSeletedItem()
    {
        selectedItem.quantity--;

        if(selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                //UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
}
