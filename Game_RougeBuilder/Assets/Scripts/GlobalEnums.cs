using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// ��Ϸ�׶�����
/// </summary>
public enum EGamePeriodType
{
    None,
    Fight, // �໥����
    SelectTalent, // ѡ���츳
}

/// <summary>
/// ��Ϸ�׶�ͨ������
/// </summary>
public enum EGamePeriodPassCondition
{
    Time, // ����ʱ��
    TalentPoolCount, // ����츳������
}