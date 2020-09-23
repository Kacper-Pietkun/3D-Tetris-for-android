using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightBehaviour : MonoBehaviour
{
    private Light light;
    private Light[] lightHelper = new Light[2];

    private void Awake()
    {
        light = transform.GetComponent<Light>();

        int i = 0;
        foreach(Transform child in transform)
        {
            lightHelper[i] = child.GetComponent<Light>();
            i++;
        }
    }

    void Start()
    {
        StartCoroutine(animationDelayCoroutine());
    }

    // Waiting 0.5 second before starting light's animation
    private IEnumerator animationDelayCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(lightIntensityCoroutine());
    }


    // Increasing intensity of the light
    private IEnumerator lightIntensityCoroutine()
    {
        while (light.intensity < 3.5)
        {
            light.intensity += 0.02f;
            lightHelper[0].intensity += 0.02f;
            lightHelper[1].intensity += 0.02f;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);
        // Opening Main Menu after playing intro
        SceneManager.LoadScene(ConstVar.sceneMainMenu);
    }

}
