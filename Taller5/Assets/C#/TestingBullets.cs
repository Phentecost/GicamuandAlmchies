using Code_Proyectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class TestingBullets : MonoBehaviour
    {
        [SerializeField] GameObject bulletPrefab;   
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.transform.parent = null;
                bullet.GetComponent<Proyectile>().SetMovement(10, new Vector3(1,0,0), false);
            }
        }
    }
}
