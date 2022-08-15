using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleCollector : MonoBehaviour
{
    private int count = 0;

    [SerializeField]
    private AudioSource collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            Destroy(collision.gameObject);
            ++count;
            GameObject appleText = GameObject.FindWithTag("Score");
            appleText.GetComponent<Text>().text = "Apples: " + count.ToString();
            collectSound.Play();
        }
    }
}
