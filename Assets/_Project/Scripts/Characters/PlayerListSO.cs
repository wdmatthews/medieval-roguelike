using System.Collections.Generic;
using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Player List", menuName = "Medieval Roguelike/Characters/Player List")]
    public class PlayerListSO : ScriptableObject
    {
        public List<Player> Players { get; set; }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }
    }
}
