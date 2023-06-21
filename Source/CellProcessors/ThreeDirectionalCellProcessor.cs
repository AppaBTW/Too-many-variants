using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    public class ThreeDirectionalCellProcessor : CellProcessor
    {
        public override string Name => "Three Directional Cell";
        public override int CellType => 12;
        public override string CellSpriteIndex => "ThreeDirectional";
        [Info(CellCategory.Push)]

        public ThreeDirectionalCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return true;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            if (force <= 0)
                return false;
            if (direction == cell.Transform.Direction)
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
            //do nothing
        }

        public override void Clear()
        {
            //do nothing
        }
    }
}