using System;
using System.Collections.Generic;
using CellEncoding;
using Indev2;
using Modding;


//MUST BE NAMED MOD
public class Mod : IMod
{
    public static Interface Interface;
    public string UniqueName => "Too many Varients";
    public string DisplayName => "Varients Mod";
    public string Author => "AppaBTW";
    public string Version => "1.0.0";
    public ILevelFormat LevelFormat => null;
    public string Description => "Adds a bunch of varients of existing cells or cells from other cell machine remakes";
    public string[] Dependencies => Array.Empty<string>();

    public void Initialize(Interface @interface)
    {
        Interface = @interface;
    }

    public IEnumerable<CellProcessor> GetCellProcessors(ICellGrid cellGrid)
    {

        //cooemnted means a little broken
        yield return new BasicCellProcessor(cellGrid);
        yield return new SlideCellProcessor(cellGrid);
        yield return new OneDirectionalCellProcessor(cellGrid);
        yield return new TwoDirectionalCellProcessor(cellGrid);
        yield return new ThreeDirectionalCellProcessor(cellGrid);
        yield return new SwitchCellProcessor(cellGrid);
        yield return new BrokenCellProcessor(cellGrid);
        yield return new FrozenPushCellProcessor(cellGrid);
        yield return new FreezeProcessor(cellGrid);
        yield return new GeneratorCellProcessor(cellGrid);
        //yield return new CrossGeneratorCellProcessor(cellGrid);
        yield return new BiGeneratorCellProcessor(cellGrid);
        yield return new SlantedGeneratorCellProcessor(cellGrid);
        yield return new CCWSlantedGeneratorCellProcessor(cellGrid);
        //yield return new SwapperCellProcessor(cellGrid);
        yield return new CWRotateProcessor(cellGrid);
        yield return new CCWRotateProcessor(cellGrid);
        yield return new HalfTurnRotateProcessor(cellGrid);
        //yield return new CWMoveratorCellProcessor(cellGrid);
        yield return new MoverCellProcessor(cellGrid);
        yield return new NudgeCellProcessor(cellGrid);
        yield return new MissileCellProcessor(cellGrid);
        yield return new WallCellProcessor(cellGrid);
        yield return new VoidProcessor(cellGrid);
        yield return new TrashCellProcessor(cellGrid);
        yield return new EnemyCellProcessor(cellGrid);
        yield return new StrongEnemyCellProcessor(cellGrid);


    }
}
