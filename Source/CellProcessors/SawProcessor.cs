using Modding;
using Modding.PublicInterfaces.Cells;
using System.Threading;

namespace Indev2
{
    public class SawCellProcessor : SteppedCellProcessor
    {
        public override string Name => "Saw";
        public override int CellType => 45;
        public override string CellSpriteIndex => "Saw";

        public SawCellProcessor(ICellGrid cellGrid) : base(cellGrid)
        {
        }

        public override bool OnReplaced(BasicCell basicCell, BasicCell existingCell)
        {
            return false;
        }

        public override bool TryPush(BasicCell cell, Direction direction, int force)
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

        public override void Step(CancellationToken ct)
        {
            foreach (var cell in GetOrderedCellEnumerable())
            {
                if (ct.IsCancellationRequested)
                    return;
                var targetPos = cell.Transform.Position + cell.Transform.Direction.Rotate(1).AsVector2Int + cell.Transform.Direction.Rotate(2).AsVector2Int;
                _cellGrid.AddCell(targetPos, cell.Transform.Direction.Rotate(1), CellType, cell.Transform);
                _cellGrid.RemoveCell(cell);
            }
        }
    }
}