using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.player.conditions.uiCondition = this;
    }
}
