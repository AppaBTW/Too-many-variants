using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class MissileCellProcessor : SteppedCellProcessor
    {
        public override string Name => "Missile Cell";
        public override int CellType => 24;
        public override string CellSpriteIndex => "Missile";

        public MissileCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell existingCell)
        {
            _cellGrid.RemoveCell(existingCell);
            return false;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
        {
            return true;
        }

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                var target = cell.Transform.Position + cell.Transform.Direction.AsVector2Int;
                var targetCell = _cellGrid.GetCell(target);
                if (!_cellGrid.InBounds(target))
                    continue;
                if (!cell.Frozen)
                {
                    if (targetCell is not null)
                    {
                        if (targetCell.Value.Instance.Type != 20)
                        {
                            _cellGrid.RemoveCell(cell);
                            _cellGrid.RemoveCell((BasicCell)targetCell);
                        }

                        continue;
                    }
                    _cellGrid.MoveCell(cell, target);
                }
            }
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