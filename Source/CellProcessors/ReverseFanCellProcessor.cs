using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class ReverseFanCellProcessor : TickedCellStepper
    {
        public override string Name => "Reverse Fan Cell";
        public override int CellType => 46;
        public override string CellSpriteIndex => "ReverseFan";

        public ReverseFanCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell existingCell)
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

        public override void OnCellInit(ref BasicCell cell)
        {
            //do nothing
        }

        public override void Clear()
        {
            //do nothing
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetCells())
            {
                if (ct.IsCancellationRequested)
                    return;
                foreach (var direction in Direction.All)
                {
                    var reverse = direction.Rotate(2);
                    var target = cell.Transform.Position + direction.AsVector2Int;
                    if (!_cellGrid.InBounds(target))
                        continue;

                    further:
                    {
                        target += direction.AsVector2Int;
                        var targetCell = _cellGrid.GetCell(target);
                        if (!_cellGrid.InBounds(target))
                            continue;
                        if (targetCell == null)
                            goto further;

                        _cellGrid.PushCell(targetCell.Value, reverse, 1);
                    }
                }
            }
        }
    }
}