using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] bool hasWeapon;

    public bool HasWeapon {  get { return hasWeapon; } }
}
