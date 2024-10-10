
/// <summary>
/// 一个游戏阶段
/// </summary>
public class GamePeriod
{
    /// <summary> 当前游戏阶段id </summary>
    public int id;

    /// <summary> 进行到阶段，执行的逻辑类型 </summary>
    public EGamePeriodType eType;

    /// <summary> 通过阶段的条件类型 </summary>
    public EGamePeriodPassCondition ePassCondition;

    /// <summary> 通过阶段的数值 </summary>
    public float passValue;
}