using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace Chips
{
    public class Chip:MonoBehaviour, IChip
    {
        
        [SerializeField] private ChipColor chipColor;
        
        public ChipColor ChipColor => chipColor;
        public Vector2Int CurrentPosition { get; set; }
    }
}