using UnityEngine;
using UnityEngine.UIElements;

namespace MedievalRoguelike.UI
{
    [AddComponentMenu("Medieval Roguelike/UI/Game HUD")]
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _playerHUDAsset;
        [SerializeField] private GameHUDSO _referenceToSelf;

        private VisualElement _container;
        private VisualElement _playerHUDContainer;
        private Label _difficultyLabel;

        private void Awake()
        {
            _container = _document.rootVisualElement;
            _playerHUDContainer = _container.Q("PlayerHUDs");
            _difficultyLabel = _container.Q<Label>("Difficulty");
            _referenceToSelf.HUD = this;
        }

        public void Hide()
        {
            _container.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
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
