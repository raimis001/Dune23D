using UnityEngine;

public class BulletRocket : Proc
{
    [SerializeField]
    float speed;
    [SerializeField]
    float damage = 30;

    [SerializeField]
    GameObject engine;


    //[SerializeField]
    bool active = false;

    Vector3 target;
    Transform parent;
    float timer = 0;

    private void Awake()
    {
        engine.SetActive(false);
    }

    public void Shot(Vector3 target)
    {
        if (active)
            return;

        parent = transform.parent;
        transform.SetParent(null, true);
        this.target = target;
        transform.forward = target - transform.position;

        engine.SetActive(true);

        timer = 1.2f;
        active = true;
    }

    void Update()
    {
        
        if (active)
        {
            timer -= Time.deltaTime;
            //transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, target) <= Mathf.Epsilon || timer <= 0)
            {
                Pool.Create("RocketExplosion", transform.position, Quaternion.identity);    
                Pool.Create("Decal",transform.position, Quaternion.identity);

                transform.SetParent(parent, true);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                engine.SetActive(false);

                if (FindClosestObject(1, out Proc enemy, 1))
                {
                    enemy.Damage(damage);
                }

                active = false;
            }
        }
    }
}
