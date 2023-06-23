using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    [Info(CellCategory.Gen)]
    public class WeakGeneratorCellProcessor : SteppedCellProcessor
    {
        public WeakGeneratorCellProcessor(ICellGrid cellGrid) : base(cellGrid) { }

        public override string Name => "Weak Generator";
        public override int CellType => 41;
        public override string CellSpriteIndex => "WeakGenerator";

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

        public override void OnCellInit(ref BasicCell cell) { }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var inputCell in GetOrderedCellEnumerable())
            {
                var generatorCell = inputCell;

                if (ct.IsCancellationRequested)
                    return;
                var copyCell = _cellGrid.GetCell(generatorCell.Transform.Position - generatorCell.Transform.Direction.AsVector2Int);
                if (copyCell is null)
                    continue;

                var targetPos = generatorCell.Transform.Position + generatorCell.Transform.Direction.AsVector2Int;

                if (!_cellGrid.InBounds(targetPos))
                    continue;

                var targetCell = _cellGrid.GetCell(targetPos);
                if (targetCell != null)
                    continue;

                var newCellTransform = generatorCell.Transform;
                newCellTransform.Direction = copyCell.Value.Transform.Direction;
                var prevTransform = newCellTransform;
                _cellGrid.AddCell(targetPos, copyCell.Value.Transform.Direction, copyCell.Value.Instance.Type, prevTransform);
            }
        }

        public override void Clear()
        {

        }
    }
}