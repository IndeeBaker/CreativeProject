using UnityEngine;
using UnityEngine.SceneManagement;
public class changeScene : MonoBehaviour
{
    public BoxCollider2D player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == player)
        {
            Debug.Log("Entered");
            SceneManager.LoadScene(sceneName: "SampleScene");
        }
    }

}