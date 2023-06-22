using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{

    public class CCWSemiRotateProcessor : SemiRotatorProcessor
    {
        public CCWSemiRotateProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "CCW SemiRotator";
        public override int CellType => 34;
        public override string CellSpriteIndex => "CCWSemiRotator";
        public override int RotationAmount => -1;


        public override void OnCellInit(ref BasicCell cell)
        {
        }

        public override void Clear()
        {

        }
    }
}