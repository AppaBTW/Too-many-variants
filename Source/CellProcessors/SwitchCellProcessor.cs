using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    [Info(CellCategory.Push)]
    public class SwitchCellProcessor : TickedCellStepper
    {
        public override string Name => "Switch Cell";
        public override int CellType => 17;
        public override string CellSpriteIndex => "Switch";

        public SwitchCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (cell.SpriteVariant == 1)
                return false;

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

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetCells())
            {
                BasicCell useCell = cell;
                if (Axis.Horizontal == cell.Transform.Direction.Axis)
                {
                    useCell.SpriteVariant = 0;
                }
                else
                {
                    useCell.SpriteVariant = 1;
                }
                _cellGrid.AddCell(useCell);
            }
        }

        public override bool OnReplaced(BasicCell SwitchCell, BasicCell replacingCell)
        {
            return true;
        }

        public override void OnCellInit(ref BasicCell cell)
        {
            if (Axis.Horizontal == cell.Transform.Direction.Axis)
            {
                cell.SpriteVariant = 0;
            }
            else
            {
                cell.SpriteVariant = 1;
            }
        }

        public override void Clear()
        {
            //do nothing
        }
    }
}