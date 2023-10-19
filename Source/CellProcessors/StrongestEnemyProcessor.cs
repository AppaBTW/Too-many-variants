using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class StrongestEnemyCellProcessor : CellProcessor
    {
        public override string Name => "Strongest Enemy Cell";
        public override int CellType => 43;
        public override string CellSpriteIndex => "StrongestEnemy";

        public StrongestEnemyCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell existingCell)
        {
            _cellGrid.RemoveCell(existingCell);
            _cellGrid.AddCell(existingCell.Transform.Position, existingCell.Transform.Direction, 14, null);
            return false;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
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