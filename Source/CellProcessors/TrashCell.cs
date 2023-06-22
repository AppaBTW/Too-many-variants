using Modding;
using Modding.PublicInterfaces.Cells;

namespace Indev2
{
    [LockRotation]
    public class TrashCellProcessor : CellProcessor
    {
        public override string Name => "Trash Cell";
        public override int CellType => 7;
        public override string CellSpriteIndex => "Trash";


        public TrashCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell replacingCell)
        {
            return false;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {

            var target = cell.Transform.Position - direction.AsVector2Int;
            var targetCell = _cellGrid.GetCell(target);
            if (targetCell is not null)
            {
                return true;
            }

            target = cell.Transform.Position + direction.AsVector2Int;
            targetCell = _cellGrid.GetCell(target);

            if (force <= 0)
                return false;

            if (!_cellGrid.InBounds(target))
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