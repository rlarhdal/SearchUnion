using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}"; // crosshair change
        return str ;
    }

    public void OnInteract()
    {
        if(data.type == ItemType.Interactable)
        {
            OpenDoor();
        }
        else
        {
            CharacterManager.Instance.player.itemData = data;
            CharacterManager.Instance.player.addItem?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OpenDoor()
    {
        gameObject.GetComponent<Door>().ChangeDoor();
    }
}
