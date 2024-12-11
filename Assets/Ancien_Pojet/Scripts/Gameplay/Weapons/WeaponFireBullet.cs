﻿using System.Diagnostics;
using UnityEngine;

namespace Gameplay.Weapons
{
    
    /// <summary>
    /// Represents a weapon that shot one bullet at a time to the closest enemy
    /// </summary>
    public class WeaponFireBullet : WeaponBase
    {

        [SerializeField] GameObject _prefab;
        [SerializeField] Vector2 _target;
        [SerializeField] float _speed;


        public WeaponFireBullet()
        {
        }
        
        public override void Update( PlayerController player )
        {
            _timerCoolDown += Time.deltaTime;

            if (_timerCoolDown < _coolDown)
                return;

            _timerCoolDown -= _coolDown;


            var playerPosition = player.transform.position;
            GameObject go = GameObject.Instantiate(_prefab, playerPosition, Quaternion.identity);
            Vector3 direction = (Vector3)player.transform.position + new Vector3(1, 0, 1);
            if (direction.sqrMagnitude > 0)
            {
                direction.Normalize();

                go.GetComponent<Bullet>().Initialize(direction, GetDamage(),_speed);
            }
        }
    }
}



