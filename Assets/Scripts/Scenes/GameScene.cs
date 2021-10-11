using UnityEngine;
using TMPro;

public class GameScene : MonoBehaviour
{
    public static GameScene Instance;

    [SerializeField]
    private MeshRenderer MeshBall;

    [SerializeField]
    private BallColors ConfigBallColors;

    [SerializeField]
    private TextMeshProUGUI TextScorePlayer;

    [SerializeField]
    private TextMeshProUGUI TextScoreRival;

    [SerializeField]
    private TextMeshProUGUI TextScoreBest;

    private int _playerScore = 0;
    private int _rivalScore = 0;

    private void Start()
    {
        Instance = this;

        int ballColor = Save.GetInt("BallColor");
        MeshBall.material.color = ConfigBallColors.Colors[ballColor];

        SetBestScoreText();
    }

    public void AddScore(bool toPlayer)
    {
        if (toPlayer)
        {
            _playerScore++;
            TextScorePlayer.text = _playerScore.ToString();

            int best = Save.GetInt("BestScore");
            if (_playerScore > best)
            {
                Save.SetInt("BestScore", _playerScore);
                SetBestScoreText();
            }
        }
        else
        {
            _rivalScore++;
            TextScoreRival.text = _rivalScore.ToString();
        }
    }

    private void SetBestScoreText()
    {
        TextScoreBest.text = Save.GetInt("BestScore").ToString();
    }
}
