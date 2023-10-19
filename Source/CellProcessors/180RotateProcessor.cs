using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    [Info(CellCategory.Tick)]
    public class HalfTurnRotateProcessor : RotatorProcessor
    {
        public HalfTurnRotateProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "180 Rotator";
        public override int CellType => 15;
        public override string CellSpriteIndex => "180Rotator";
        public override int RotationAmount => 2;

        public override void OnCellInit(ref BasicCell cell)
        {
        }

        public override void Clear()
        {
        }
    }
}