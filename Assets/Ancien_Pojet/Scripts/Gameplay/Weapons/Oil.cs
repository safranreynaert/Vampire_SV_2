using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    bool _fire = true;
    public bool Fire;

    private void OnTriggerStay(Collider col)
    {
        var other = HitWithParent.GetComponent<EnemyController>(col);
        if (other != null)
        {
            if (Fire == true)
            {
                GameObject.Destroy(gameObject);
                other.Burn(_fire);
            }
        }


    }
}
