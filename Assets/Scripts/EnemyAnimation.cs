using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Transform sprite;
    [SerializeField] private float speed;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;

    private float activeSize;
    // Start is called before the first frame update
    void Start()
    {
        activeSize = maxSize;

        speed = speed * Random.Range(.75f, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.localScale = Vector3.MoveTowards(sprite.localScale, Vector3.one * activeSize, speed * Time.deltaTime);

        if (sprite.localScale.x == activeSize)
        {
            if (activeSize == minSize)
            {
                activeSize = maxSize;
            }
            else
            {
                activeSize = minSize;
            }
        }
    }
}
