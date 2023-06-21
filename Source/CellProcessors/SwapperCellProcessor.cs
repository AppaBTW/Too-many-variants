using System.Threading;
using Modding;
using Modding.PublicInterfaces.Cells;
using UnityEngine;

namespace Indev2
{
    public class SwapperCellProcessor: TickedCellStepper
    {
        public SwapperCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override string Name => "Swapper";
        public override int CellType => 19;
        public override string CellSpriteIndex => "Swapper";

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
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
            foreach (var cell in GetCells())
            {
                var targetPos = cell.Transform.Position + cell.Transform.Direction.AsVector2Int;
                var targetCell = _cellGrid.GetCell(targetPos);
                if (targetCell is not null)
                {
                    continue;
                }
                var swapPos = cell.Transform.Position - cell.Transform.Direction.AsVector2Int;
                var swapCell = _cellGrid.GetCell(swapPos);
                if (swapCell is not null)
                {
                    BasicCell UseCell = (BasicCell)swapCell;
                    BasicCell targetUseCell = (BasicCell)targetCell;
                    UseCell.Transform = targetCell.Value.Transform;
                    targetUseCell.Transform = swapCell.Value.Transform;
                    _cellGrid.AddCell(UseCell);
                    _cellGrid.AddCell(targetUseCell);
                }


            }
        }
        public override void Clear()
        {

        }
    }
}