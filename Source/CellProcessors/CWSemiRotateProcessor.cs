using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{

    public class CWSemiRotateProcessor : SemiRotatorProcessor
    {
        public CWSemiRotateProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "CW SemiRotator";
        public override int CellType => 33;
        public override string CellSpriteIndex => "CWSemiRotator";
        public override int RotationAmount => 1;


        public override void OnCellInit(ref BasicCell cell)
        {
        }

        public override void Clear()
        {

        }
    }
}