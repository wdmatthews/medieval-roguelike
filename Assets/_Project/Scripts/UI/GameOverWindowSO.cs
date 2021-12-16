using UnityEngine;

namespace MedievalRoguelike.UI
{
    [CreateAssetMenu(fileName = "Game Over Window", menuName = "Medieval Roguelike/UI/Game Over Window")]
    public class GameOverWindowSO : ScriptableObject
    {
        public GameOverWindow Window { get; set; }
    }
}
