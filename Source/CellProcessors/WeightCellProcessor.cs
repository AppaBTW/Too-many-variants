using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    [Info(CellCategory.Push)]
    public class WeightCellProcessor : CellProcessor
    {
        public override string Name => "Weight Cell";
        public override int CellType => 40;
        public override string CellSpriteIndex => "Weight";

        public WeightCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (cell.Frozen == false)
                if (force >= 1)
                    force--;
            if (force == -1)
            {
                if (!_cellGrid.InBounds(cell.Transform.Position + direction.AsVector2Int))
                    return false;
                return true;
            }

            if (force <= 0)
                return false;

            var target = cell.Transform.Position + direction.AsVector2Int;
            if (!_cellGrid.InBounds(target))
                return false;
            var targetCell = _cellGrid.GetCell(target);

            if (targetCell is null)
            {
                _cellGrid.MoveCell(cell, target);
                return true;
            }

            if (!_cellGrid.PushCell(targetCell.Value, direction, force))
                return false;

            _cellGrid.MoveCell(cell, target);
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