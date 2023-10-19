using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class ChainPullerCellProcessor : SteppedCellProcessor
    {
        public ChainPullerCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Chain Puller";
        public override int CellType => 67;
        public override string CellSpriteIndex => "ChainPuller";

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
            var target = cell.Transform.Position + direction.AsVector2Int;

            if (!cell.Frozen)
                if (direction == cell.Transform.Direction)
                    force++;
                else if (direction.Axis == cell.Transform.Direction.Axis)
                    force--;
            if (direction != cell.Transform.Direction)
            {
                if (force <= 0)
                    return false;
            }

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
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if (ct.IsCancellationRequested)
                    return;
                var targetPos = cell.Transform.Position - cell.Transform.Direction.AsVector2Int;
                var targetCell = _cellGrid.GetCell(targetPos);
                var targetFront = cell.Transform.Position + cell.Transform.Direction.AsVector2Int;

                if (!_cellGrid.InBounds(targetFront))
                    continue;
                if (targetCell is null | !_cellGrid.InBounds(targetPos))
                {
                    targetCell = _cellGrid.GetCell(targetFront);
                    if (targetCell is null)
                    {
                        _cellGrid.MoveCell(cell, targetFront);
                    }
                    continue;
                }

                if (!_cellGrid.PushCell(targetCell.Value, cell.Transform.Direction, 1))
                {
                    targetCell = _cellGrid.GetCell(targetFront);
                    if (targetCell is null)
                    {
                        _cellGrid.MoveCell(cell, targetFront);
                    }
                    continue;
                }

                if (targetCell.Value.Instance.Type == 8 | targetCell.Value.Instance.Type == 14 | targetCell.Value.Instance.Type == 17)
                {
                    _cellGrid.RemoveCell((BasicCell)targetCell);
                    _cellGrid.RemoveCell(cell);
                    continue;
                }

                if (targetCell is not null && targetCell.Value.Instance.Type != 28 && targetCell.Value.Instance.Type != 30)
                {
                    var frontCell = _cellGrid.GetCell(targetFront);
                    if (frontCell is null)
                        _cellGrid.PushCell(targetCell.Value, cell.Transform.Direction, 1);
                }
            }
        }

        public override void Clear()
        {
        }
    }
}