using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class WeakMoverCellProcessor : SteppedCellProcessor
    {
        public WeakMoverCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Weak Mover";
        public override int CellType => 28;
        public override string CellSpriteIndex => "WeakMover";

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            var target = cell.Transform.Position - direction.AsVector2Int;
            var targetCell = _cellGrid.GetCell(target);

            if (targetCell is null)
            {
                target = cell.Transform.Position + direction.AsVector2Int;
                if (!_cellGrid.InBounds(target))
                    return false;
                targetCell = _cellGrid.GetCell(target);
                if (targetCell is null)
                {
                    _cellGrid.MoveCell(cell, target);
                    return true;
                }
                if (!_cellGrid.PushCell(targetCell.Value, direction, 1))
                    return false;

                _cellGrid.MoveCell(cell, target);
                return true;
            }

            if (!cell.Frozen)
                if (direction == cell.Transform.Direction)
                    force++;
                else if (direction.Axis == cell.Transform.Direction.Axis)
                    force--;

            if (force <= 0)
                return false;

            target = cell.Transform.Position + direction.AsVector2Int;
            if (!_cellGrid.InBounds(target))
                return false;
            targetCell = _cellGrid.GetCell(target);


            BasicCell useCell;
            if (targetCell is null)
            {
                useCell = cell;

                _cellGrid.MoveCell(useCell, target);
                return true;
            }
            return false;
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