using UnityEngine;
using System.Collections;

public class AnimalMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDuration = 1f;
    public float idleDuration = 2f;

    private Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Pick a random direction
            Vector2 direction = directions[Random.Range(0, directions.Length)];
            float elapsed = 0f;

            // Move for moveDuration
            while (elapsed < moveDuration)
            {
                rb.velocity = direction * moveSpeed;
                elapsed += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;

            // Wait idle for a bit
            yield return new WaitForSeconds(idleDuration);
        }
    }
}
