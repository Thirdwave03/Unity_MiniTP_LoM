using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFitter : MonoBehaviour
{
    private void Start()
    {
        Camera cam = Camera.main;
        if(cam == null || !cam.orthographic)
        {
            Debug.LogError("Cam could not be found or its mode is not Orthographic");
            return;
        }

        float worldScreenHeight = cam.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * cam.aspect;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if( sr == null )
        {
            Debug.LogError("Could not find SpriteRenderer component");
            return;
        }

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        Vector3 newScale = transform.localScale;
        newScale.x = worldScreenWidth / spriteWidth;
        newScale.y = worldScreenHeight / spriteHeight;
        transform.localScale = newScale;
    }





}
