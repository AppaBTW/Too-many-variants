using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class TunnelCellProcessor : CellProcessor
    {
        public override string Name => "Tunnel";
        public override int CellType => 72;
        public override string CellSpriteIndex => "Tunnel";

        public TunnelCellProcessor(ICellGrid cellGrid) : base(cellGrid)
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
            if (direction.Axis == cell.Transform.Direction.Axis)
            {
                if (_cellGrid.GetCell(cell.Transform.Position - direction.AsVector2Int) is not null)
                {
                    if (targetCell is not null)
                    {
                        if (!_cellGrid.PushCell(targetCell.Value, direction, force))
                            return false;
                    }
                    BasicCell refrenceCell = _cellGrid.GetCell(cell.Transform.Position - direction.AsVector2Int).Value;
                    _cellGrid.RemoveCell(refrenceCell);
                    refrenceCell.Transform = refrenceCell.Transform.SetPosition(target);
                    _cellGrid.AddCell(refrenceCell);
                    return false;
                }
            }

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