using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Represents an enemy who's moving toward the player
/// and damage him on collision
/// data bout the enemy are stored in the EnemyData class
/// CAUTION : don't forget to call Initialize when you create an enemy
/// </summary>
public class EnemyController : Unit
{
    public GameObject _elecVisu;
    private Material M_base;
    public Material M_burn;
    public GameObject Skin;

    float _animburn;
    GameObject _player;
    Rigidbody _rb;
    EnemyData _data;
    private List<PlayerController> _playersInTrigger = new List<PlayerController>();
    private float _timerBurn = 0;
    private float _timerStun = 0;
    private float _time;
    private float _burnTime;
    private float _stunTime;
    private float _burnDot;
    public bool _stun;
    public bool _burn;
    GameObject go = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void Initialize(GameObject player, EnemyData data)
    {
        _player = player;
        _data = data;
        _life = data.Life;
        _team = 1;
        _burnDot = data.BurnDot;
        _burnTime = data.BurnTime;
        _stunTime = data.StunTime;
        M_base = Skin.GetComponent<MeshRenderer>().material;

    }

    private void Update()
    {
        transform.LookAt(_player.transform.position);
        if (_life <= 0)
            return;

        foreach (var player in _playersInTrigger)
        {
            player.Hit(Time.deltaTime * _data.DamagePerSeconds);
        }
        DotBurn();
        GameObject.Destroy(go, 5);
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        if (_stun == false)
        {
            GameObject.Destroy(go);
            Vector3 direction = _player.transform.position - transform.position;
            direction.y = 0f;

            float moveStep = _data.MoveSpeed * Time.deltaTime;
            if (moveStep >= direction.magnitude)
            {
                _rb.velocity = Vector3.zero;
                transform.position = _player.transform.position;
            }
            else
            {
                direction.Normalize();
                _rb.velocity = direction * _data.MoveSpeed;
            }
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition;

            _timerStun += Time.deltaTime - _time;

            if (_timerStun > _stunTime)
            {
                _stun = false;
                _rb.constraints = RigidbodyConstraints.None;
                _rb.constraints = RigidbodyConstraints.FreezePositionY;
                _timerStun = 0;
            }
        }
        
    }
    private void DotBurn()
    {
        if(_burn == true)
        {
            float _dot = 0;

            if (_animburn >= 0.2f + _dot)
                Skin.GetComponent<MeshRenderer>().material = M_base;

            if (_timerBurn >= 1)
            {
                Skin.GetComponent<MeshRenderer>().material = M_burn;
                _dot++;


                _life -= _burnDot;
                _timerBurn = 0;
                if (_life <= 0)
                {
                    Die();
                }
            }
            _timerBurn += Time.deltaTime - _time;
            _animburn += Time.deltaTime - _time;

            if (_timerBurn > _burnTime)
            {
                _timerBurn = 0;
                _burn = false;
            }
        }
    }
    public override void Hit(float damage)
    {
        _life -= damage;

        if (Life <= 0)
        {
            Die();
        }
    }

    public override void Burn(bool _fire)
    {
        _burn = _fire;
        _animburn = 0;
        if (_burn == false)
        {
            _time = Time.deltaTime;
        }
    }

    public override void Stun(bool _electricity)
    {
        _stun = _electricity;
        go = GameObject.Instantiate(_elecVisu, transform.position, Quaternion.identity);
        if (_stun == false)
        {
            _time = Time.deltaTime;
            
        }
    }


    void Die()
    {
        GameObject.Destroy(go);
        MainGameplay.Instance.Enemies.Remove(this);
        GameObject.Destroy(gameObject);
        var xp = GameObject.Instantiate(MainGameplay.Instance.PrefabXP, transform.position, Quaternion.identity);
        xp.GetComponent<CollectableXp>().Initialize(1);
    }

    private void OnTriggerEnter(Collider col)
    {
        var other = HitWithParent.GetComponent<PlayerController>(col);
        
        if (other != null)
        {
            if (_playersInTrigger.Contains(other) == false)
                _playersInTrigger.Add(other);
        }
        
    }
    private void OnTriggerStay(Collider col)
    {
        var waterArea = HitWithParent.GetComponent<Water>(col);
        var oilArea = HitWithParent.GetComponent<Oil>(col);
        if (waterArea == true)
        {
            if (_stun == true)
            {
                waterArea.GetComponent<Water>().Stun = true;
            }
        }
        if (oilArea == true)
        {
            if (_burn == true)
            {
                oilArea.GetComponent<Oil>().Fire = true;
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        var other = HitWithParent.GetComponent<PlayerController>(col);

        if (other != null)
        {
            if (_playersInTrigger.Contains(other))
                _playersInTrigger.Remove(other);
        }
    }
}