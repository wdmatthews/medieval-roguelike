using UnityEngine;
using UnityEngine.UIElements;

namespace MedievalRoguelike.UI
{
    [AddComponentMenu("Medieval Roguelike/UI/Game Over Window")]
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private GameOverWindowSO _referenceToSelf;

        private VisualElement _container;
        private Label _difficultyLabel;
        private Label _roomsClearedLabel;
        private Label _pointsLabel;
        private Label _killsLabel;
        private Button _continueButton;

        public System.Action Continue { get; set; }

        private void Awake()
        {
            _container = _document.rootVisualElement;
            _difficultyLabel = _container.Q<Label>("Difficulty");
            _roomsClearedLabel = _container.Q<Label>("RoomsCleared");
            _pointsLabel = _container.Q<Label>("Points");
            _killsLabel = _container.Q<Label>("Kills");
            _continueButton = _container.Q<Button>("Continue");
            _continueButton.clicked += OnContinue;
            _referenceToSelf.Window = this;
            Close();
        }

        public void Open(int difficulty, int totalRoomsCleared, int totalPoints, int totalKills)
        {
            _difficultyLabel.text = $"Difficulty {difficulty}";
            _roomsClearedLabel.text = $"{totalRoomsCleared} Rooms Cleared";
            _pointsLabel.text = $"{totalPoints} Points";
            _killsLabel.text = $"{totalKills} Kills";
            _container.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        private void Close()
        {
            _container.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        private void OnContinue()
        {
            Continue?.Invoke();
        }
    }
}
