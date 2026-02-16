using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("PlayerBase").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerBase")
        {
            GameObject.Find("PlayerBase").GetComponent<baseScript>().baseHP -= 10;
            Destroy(this.gameObject);
        }
    }
}
