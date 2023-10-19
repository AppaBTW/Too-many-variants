using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class EliminatorCellProcessor : SteppedCellProcessor
    {
        public EliminatorCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Eliminator";
        public override int CellType => 41;
        public override string CellSpriteIndex => "Eliminator";

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
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var inputCell in GetOrderedCellEnumerable())
            {
                var targetPos = inputCell.Transform.Position + inputCell.Transform.Direction.AsVector2Int;
                if (ct.IsCancellationRequested)
                    return;
                var copyCell = _cellGrid.GetCell(inputCell.Transform.Position - inputCell.Transform.Direction.AsVector2Int);
                var targetCell = _cellGrid.GetCell(targetPos);
                if (copyCell is null)
                    continue;
                if (targetCell is null)
                    continue;

                if (!_cellGrid.InBounds(targetPos))
                    continue;

                if (targetCell == null && targetCell.Value.Instance.Type == 20)
                    continue;
                if (targetCell is null)
                    continue;
                if (copyCell.Value.Instance.Type == targetCell.Value.Instance.Type)
                    _cellGrid.RemoveCell(targetPos);
            }
        }

        public override void Clear()
        {
        }
    }
}