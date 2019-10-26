using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMiner : Unit
{
    void Start(){
        _type = unitType.Miner;
        _hp = 50.0f;
        _speed = 5.0f;
        _attack = 3.0f;
        _attackSpeed = 3.0f;
        _defense = 1.0f;
}

}
