//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class StartButton : MonoBehaviour
//{
//    public void StartGame()
//    {
//        // FTUE logic (keep it)
//        if (PlayerData.instance.ftueLevel == 0)
//        {
//            PlayerData.instance.ftueLevel = 1;
//            PlayerData.instance.Save();
//        }

//        // Load main scene
//        SceneManager.LoadScene("main");

//        // Tell GameManager to start the game automatically
//        StartCoroutine(StartGameAfterLoad());
//    }

//    private System.Collections.IEnumerator StartGameAfterLoad()
//    {
//        // wait one frame so GameManager initializes
//        yield return null;

//        GameManager gm = FindObjectOfType<GameManager>();
//        if (gm != null)
//        {
//            gm.SwitchState("Game");
//        }
//        else
//        {
//            Debug.LogError("GameManager not found!");
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif
#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        if (PlayerData.instance.ftueLevel == 0)
        {
            PlayerData.instance.ftueLevel = 1;
            PlayerData.instance.Save();
#if UNITY_ANALYTICS
            AnalyticsEvent.FirstInteraction("start_button_pressed");
#endif
        }

#if UNITY_PURCHASING
        var module = StandardPurchasingModule.Instance();
#endif
        SceneManager.LoadScene("main");
    }
}
