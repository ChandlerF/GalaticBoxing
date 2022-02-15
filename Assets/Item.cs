using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo ItemInfo;
    public GameObject ItemGameObject;


    public abstract void Use();
}
