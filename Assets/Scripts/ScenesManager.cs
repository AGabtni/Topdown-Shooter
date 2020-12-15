using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{

    lobby,
    level0,
    level1,

}
public class ScenesManager : GenericSingletonClass<ScenesManager>
{

    Scenes currentScene;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);


    }


    

   public void ReloadScene(){
        
        //SceneManager.LoadScene(SceneManager.);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void LoadNextLevel()
    {
        
        //SceneManager.LoadScene(settings.sceneName+1.ToString());

    }



}
