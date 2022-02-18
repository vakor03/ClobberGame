using UnityEngine;
using Vakor.ClobberGame.Lib.Chips;

namespace Chips
{
    public interface IChip
    {
        ChipColor ChipColor { get; }
        Vector2Int CurrentPosition { get; set; }
    }
}