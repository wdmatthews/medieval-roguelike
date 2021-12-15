using UnityEngine;
using UnityEngine.UIElements;

namespace MedievalRoguelike.UI
{
    [AddComponentMenu("Medieval Roguelike/UI/Game HUD")]
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _playerHUDAsset;
        [SerializeField] private GameHUDSO _gameHUD;

        private VisualElement _playerHUDContainer;
        private Label _difficultyLabel;

        private void Awake()
        {
            VisualElement container = _document.rootVisualElement;
            _playerHUDContainer = container.Q("PlayerHUDs");
            _difficultyLabel = container.Q<Label>("Difficulty");
            _gameHUD.HUD = this;
        }

        public PlayerHUD AddPlayerHUD(string name, float healthPercent)
        {
            PlayerHUD playerHUD = new();
            playerHUD.Initialize(_playerHUDAsset, _playerHUDContainer, name, healthPercent);
            return playerHUD;
        }

        public void UpdateDifficulty(int difficulty)
        {
            _difficultyLabel.text = $"{difficulty}";
        }
    }
}
