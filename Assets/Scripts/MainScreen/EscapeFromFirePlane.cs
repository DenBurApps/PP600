using UnityEngine.SceneManagement;

public class EscapeFromFirePlane : MainMenuGamePlane
{
    private void OnEnable()
    {
        SceneName = "EscapeFromFireScene";
        MainScreen.EscapeFromFireClicked += Enable;
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        MainScreen.EscapeFromFireClicked -= Enable;
        UnsubscribeToEvents();
    }
}