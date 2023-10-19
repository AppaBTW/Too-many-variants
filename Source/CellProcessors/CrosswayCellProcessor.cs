using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class CrosswayCellProcessor : CellProcessor
    {
        public override string Name => "Crossway";
        public override int CellType => 73;
        public override string CellSpriteIndex => "Crossway";

        public CrosswayCellProcessor(ICellGrid cellGrid) : base(cellGrid)
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
            // tunneling
            if (_cellGrid.GetCell(cell.Transform.Position - direction.AsVector2Int) is null)
            {
                // normal pushing
                if (force <= 0)
                    return false;

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
            if (targetCell is not null && !_cellGrid.PushCell(targetCell.Value, direction, force))
                return false;
            BasicCell refrenceCell = _cellGrid.GetCell(cell.Transform.Position - direction.AsVector2Int).Value;
            _cellGrid.RemoveCell(refrenceCell);
            refrenceCell.Transform = refrenceCell.Transform.SetPosition(target);
            _cellGrid.AddCell(refrenceCell);
            return false;
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