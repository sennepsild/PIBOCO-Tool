using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public SceneHolderSO SceneConnectionData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlyToPlanet(int number)
    {
        string currentScene= SceneManager.GetActiveScene().name;

        string SceneToChangeTo ="";

        for (int i = 0; i < SceneConnectionData.CurrentScene.Length; i++)
        {
            if(SceneConnectionData.CurrentScene[i] == currentScene && SceneConnectionData.ConnectionNumber[i] == number)
            {
                SceneToChangeTo = SceneConnectionData.NextScene[i];
            }
        }
        if (SceneToChangeTo != "")
            SceneManager.LoadScene(SceneToChangeTo);
        else
            Debug.LogError("No Scene Conncetion Found");
    }
    


}
