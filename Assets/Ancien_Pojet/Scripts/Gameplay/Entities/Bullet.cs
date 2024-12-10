using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Represents a bullet moving in a given direction
/// A bullet can be fired by the player or by an enemy
/// call Initialize to set the direction
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] int _team;
    [SerializeField] float _timeToLive = 10.0f;
    [SerializeField] protected bool burn;
    [SerializeField] protected bool stun;
    [SerializeField] protected bool _oil;
    [SerializeField] protected bool _water;

    float _speed = 10;
    float _damage = 5;
    bool _fire = true;
    bool _electricity = true;
    Vector3 _direction;

    public void Initialize(Vector3 direction , float damage , float speed )
    {
        _direction = direction;
        _speed = speed;
        _damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, _timeToLive);
    }

    void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider col)
    {
        var other = HitWithParent.GetComponent<Unit>(col);
        var enemy = HitWithParent.GetComponent<EnemyController>(col);

        if (other == null)
        {
            if (_water == false && _oil == false)
            {
                GameObject.Destroy(gameObject);
            }
        }
        
        else if (other.Team != _team)
        {
            if(_water == false && _oil == false)
            {
                GameObject.Destroy(gameObject);
            }

            other.Hit(_damage);

            if(burn == true)
            {
                other.Burn(_fire);
            }

            if(stun == true) 
            {
                other.Stun(_electricity);
            }
        }
        
    }
}