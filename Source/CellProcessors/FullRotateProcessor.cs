using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class FullRotateProcessor : RotatorProcessor
    {
        public FullRotateProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "360 Rotator";
        public override int CellType => 69;
        public override string CellSpriteIndex => "360Rotator";
        public override int RotationAmount => 4;

        public override void OnCellInit(ref BasicCell cell)
        {
        }

        public override void Clear()
        {
        }
    }
}