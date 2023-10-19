using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class InclusiveMoverCellProcessor : SteppedCellProcessor
    {
        public InclusiveMoverCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Inclusive Mover";
        public override int CellType => 26;
        public override string CellSpriteIndex => "InclusiveMover";

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (force == -1)
            {
                if (!_cellGrid.InBounds(cell.Transform.Position + direction.AsVector2Int))
                    return false;
                return true;
            }
            if (!cell.Frozen)
                if (direction == cell.Transform.Direction)
                    force++;
                else if (direction.Axis == cell.Transform.Direction.Axis)
                    force--;

            if (force <= 0)
                return false;

            var target = cell.Transform.Position + direction.AsVector2Int;
            if (!_cellGrid.InBounds(target))
                return false;
            var targetCell = _cellGrid.GetCell(target);

            BasicCell useCell;
            if (targetCell is null)
            {
                return false;
            }

            if (!_cellGrid.PushCell(targetCell.Value, direction, force))
                return false;

            useCell = cell;

            _cellGrid.MoveCell(useCell, target);
            return true;
        }

        public override void OnCellInit(ref BasicCell cell)
        {
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if (ct.IsCancellationRequested)
                    return;
                BasicCell swapCell = cell;
                _cellGrid.PushCell(swapCell, swapCell.Transform.Direction, 0);
            }
        }

        public override void Clear()
        {
        }
    }
}