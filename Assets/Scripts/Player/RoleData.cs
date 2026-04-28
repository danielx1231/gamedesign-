using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RoleData
{
    public int maxHp=100;
    public string name;
    public int hp=100;
    public int attack=10;
    public int def=1;
    public int level=1;
    public float jie;
    public int maxExp;
    public string path;
    public float speed=2;

    public int equipHp;
    internal int mp;
    internal int gold;
}
