using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerConditions conditions;

    public ItemData itemData;
    public Action addItem;

    public Rigidbody _rigidbody;

    private void Awake()
    {
        CharacterManager.Instance.player = this;
        controller = GetComponent<PlayerController>();
        conditions = GetComponent<PlayerConditions>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}
