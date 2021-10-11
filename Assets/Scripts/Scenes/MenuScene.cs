using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    [SerializeField]
    private GameObject CanvasDynamicMainMenu;

    [SerializeField]
    private GameObject CanvasDynamicSettings;

    [SerializeField]
    private GameObject CanvasDynamicWaitingForConnection;

    [SerializeField]
    private GameObject CanvasMainMenu;

    [SerializeField]
    private GameObject CanvasSettings;

    [SerializeField]
    private GameObject CanvasStaticWaitingForConnection;

    [SerializeField]
    private Image ImageBall;

    [SerializeField]
    private BallColors ConfigBallColors;

    private int _currentColor;

    private void Start()
    {
        _currentColor = Save.GetInt("BallColor");
        SetBallColor();
    }

    public void ClickHost()
    {
        ShowWaitingScreen();

        NetworkManager.Instance.HostGame();
    }

    public void ClickJoin()
    {
        ShowWaitingScreen();

        NetworkManager.Instance.JoinGame();
    }

    public void ClickSettings()
    {
        HideMainMenu();

        CanvasSettings.SetActive(true);
        CanvasDynamicSettings.SetActive(true);
    }

    public void ClickChangeColor(int dir)
    {
        _currentColor += dir;

        if (_currentColor == -1)
            _currentColor = ConfigBallColors.Colors.Length - 1;
        else if (_currentColor == ConfigBallColors.Colors.Length)
                _currentColor = 0;

        Save.SetInt("BallColor", _currentColor);

        SetBallColor();
    }

    public void ClickBack()
    {
        CanvasSettings.SetActive(false);
        CanvasDynamicSettings.SetActive(false);

        CanvasMainMenu.SetActive(true);
        CanvasDynamicMainMenu.SetActive(true);
    }

    private void ShowWaitingScreen()
    {
        HideMainMenu();

        CanvasDynamicWaitingForConnection.SetActive(true);
        CanvasStaticWaitingForConnection.SetActive(true);
    }

    private void HideMainMenu()
    {
        CanvasMainMenu.SetActive(false);
        CanvasDynamicMainMenu.SetActive(false);
    }

    private void SetBallColor()
    {
        ImageBall.color = ConfigBallColors.Colors[_currentColor];
    }
}
