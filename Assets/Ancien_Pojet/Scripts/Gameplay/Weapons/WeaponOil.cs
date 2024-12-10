using UnityEngine;

namespace Gameplay.Weapons
{

    /// <summary>
    /// Represents a lasso with a large AOE
    /// </summary>
    public class WeaponOil : WeaponBase
    {

        [SerializeField] GameObject _prefab;

        public WeaponOil()
        {
        }

        public override void Update(PlayerController player)
        {
            _timerCoolDown += Time.deltaTime;

            if (_timerCoolDown < _coolDown)
                return;

            _timerCoolDown -= _coolDown;


            Vector3 position = (Vector3)player.transform.position + Vector3.right * player.DirectionX * 2;

            GameObject go = GameObject.Instantiate(_prefab, position, Quaternion.identity);

            go.GetComponent<Bullet>().Initialize(new Vector3(),GetDamage(),0);

        }
    }
}