using UnityEngine.SceneManagement;

public class PickTheFruitPlane : MainMenuGamePlane
{
    private void OnEnable()
    {
        SceneName = "PickTheFruitScene";
        MainScreen.PickTheFruitClicked += Enable;
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        MainScreen.PickTheFruitClicked -= Enable;
        UnsubscribeToEvents();
    }
}
