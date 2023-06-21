using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    [Info(CellCategory.Push)]
    public class BrokenCellProcessor : CellProcessor
    {
        public override string Name => "Broken Cell";
        public override int CellType => 17;
        public override string CellSpriteIndex => "Broken";


        public BrokenCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            return true;
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override void OnCellInit(ref BasicCell cell)
        {
            //do nothing
        }

        public override void Clear()
        {
            //do nothing
        }
    }
}