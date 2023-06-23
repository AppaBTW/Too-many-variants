using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class CWMoveratorCellProcessor : SteppedCellProcessor
    {
        public CWMoveratorCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "CW Moverator";
        public override int CellType => 27;
        public override string CellSpriteIndex => "CWMoverator";

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
            if(!cell.Frozen)
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
                var RotationAmount = 1;
                foreach (var direction in Direction.All)
                {
                    var target = cell.Transform.Position + direction.AsVector2Int;
                    var targetCell = _cellGrid.GetCell(target);

                    if (targetCell == null)
                        continue;
                    if (targetCell.Value.Instance.Type == 20)
                        continue;
                    if (targetCell.Value.Instance.Type == 26 | targetCell.Value.Instance.Type == 2 | targetCell.Value.Instance.Type == 22 | targetCell.Value.Instance.Type == 23 | targetCell.Value.Instance.Type == 24)
                    {
                        BasicCell useCell;
                        useCell = (BasicCell)targetCell;
                        _cellGrid.RemoveCell(useCell);
                        useCell.Transform = useCell.Transform.Rotate(RotationAmount);
                        _cellGrid.AddCell(useCell);
                        continue;
                    }

                    _cellGrid.RotateCell(targetCell.Value, targetCell.Value.Transform.Direction.Rotate(RotationAmount));
                }
                _cellGrid.PushCell(cell, cell.Transform.Direction, 0);
            }
        }

        public override void Clear()
        {

        }
    }
}