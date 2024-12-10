using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    public float bulletDamage;
    private float shotDir;

    private Transform player;

    private void OnEnable()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        shotDir = player.localScale.x;
        StartCoroutine(DestoryTimer());
    }

    private void Update()
    {
        transform.position += Vector3.right * shotDir * bulletSpeed * Time.deltaTime;
    }

    private IEnumerator DestoryTimer()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

}
