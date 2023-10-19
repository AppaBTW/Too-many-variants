using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    [Info(CellCategory.Push)]
    public class FrozenPushCellProcessor : SteppedCellProcessor
    {
        public override string Name => "Frozen Push Cell";
        public override int CellType => 19;
        public override string CellSpriteIndex => "FrozenPush";

        public FrozenPushCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
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
            if (!_cellGrid.InBounds(target))
                return false;
            var targetCell = _cellGrid.GetCell(target);

            if (force <= 0)
                return false;

            if (targetCell is null)
            {
                BasicCell Usecell = cell;
                Usecell.SpriteVariant = (short)direction.AsInt;
                _cellGrid.AddCell(Usecell);
                _cellGrid.MoveCell(Usecell, target);
                return true;
            }
            else
            {
                BasicCell Usecell = cell;
                Usecell.SpriteVariant = (short)direction.AsInt;
                _cellGrid.AddCell(Usecell);
                _cellGrid.MoveCell(Usecell, target);
                return true;
            }
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if (ct.IsCancellationRequested)
                    return;
                BasicCell dirCell = cell;
                dirCell.SpriteVariant = (short)cell.Transform.Direction.AsInt;
                dirCell.Transform = dirCell.Transform.SetDirection(Direction.Right);
                dirCell.Transform = dirCell.Transform.Rotate(dirCell.SpriteVariant);

                _cellGrid.PushCell(cell, dirCell.Transform.Direction, 1);
            }
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
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
    }
}