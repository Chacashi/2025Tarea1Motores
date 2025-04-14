using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorCotroller : MonoBehaviour
{
    public static event Action OnDetectorActivate;
    public static event Action OnDetectorDesactivate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnDetectorActivate?.Invoke();
         
          
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnDetectorDesactivate?.Invoke();
        }


    }

}
