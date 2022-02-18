namespace Vakor.ClobberGame.Lib.Chips
{
    public class Chip:IChip
    {
        public Chip(ChipColor chipColor)
        {
            ChipColor = chipColor;
        }

        public ChipColor ChipColor { get; }
        

        public override string ToString()
        {
            return ChipColor.ToString();
        }
    }
}