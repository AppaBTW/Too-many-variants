using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class SplitterCellProcessor : SteppedCellProcessor
    {
        public SplitterCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Splitter Cell";
        public override int CellType => 39;
        public override string CellSpriteIndex => "Splitter";

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

            if (targetCell is null)
            {
                _cellGrid.MoveCell(cell, target);
                return true;
            }

            if (!_cellGrid.PushCell(targetCell.Value, direction, -1))
                return false;

            //handles down
            var upPos = targetCell.Value.Transform.Position + targetCell.Value.Transform.Direction.Rotate(1).AsVector2Int;
            var upCell = _cellGrid.GetCell(upPos);
            if (upCell is not null)
                _cellGrid.PushCell(upCell.Value, targetCell.Value.Transform.Direction.Rotate(1), 1);
            BasicCell placeCell = targetCell.Value;
            placeCell.Transform = placeCell.Transform.Rotate(1);
            placeCell.Transform = placeCell.Transform.SetPosition(upPos);
            _cellGrid.AddCell(placeCell);

            // handles up
            upPos = targetCell.Value.Transform.Position + targetCell.Value.Transform.Direction.Rotate(-1).AsVector2Int;
            upCell = _cellGrid.GetCell(upPos);
            if (upCell is not null)
                _cellGrid.PushCell(upCell.Value, targetCell.Value.Transform.Direction.Rotate(-1), 1);
            placeCell = targetCell.Value;
            placeCell.Transform = placeCell.Transform.Rotate(-1);
            placeCell.Transform = placeCell.Transform.SetPosition(upPos);
            _cellGrid.AddCell(placeCell);

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
                _cellGrid.PushCell(cell, cell.Transform.Direction, 1);
            }
        }

        public override void Clear()
        {
        }
    }
}