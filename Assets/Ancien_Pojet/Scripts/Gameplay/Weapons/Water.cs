using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    bool _electricity = true;
    public bool Stun;

    private void OnTriggerStay(Collider col)
    {
        var other = HitWithParent.GetComponent<EnemyController>(col);
        if(other != null)
        {
            if (Stun == true)
            {
                GameObject.Destroy(gameObject);
                other.Stun(_electricity);
            }
        }
        

    }
}
