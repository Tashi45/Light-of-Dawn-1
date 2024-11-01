using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    

    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName);
        }
        else if (changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
        

    }
}
