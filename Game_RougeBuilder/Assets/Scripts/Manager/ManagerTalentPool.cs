using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Talent
{
    public int id;
    public int buffDataId;

    public UITalentCtrl ui;

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetBuffDataId(int buffDataId)
    {
        this.buffDataId = buffDataId;

        var configBuff = ManagerConfig.Instance.GetExcel<BuffExcelData>();
        var dataBuff = configBuff.GetExcelItem(buffDataId);

        var desc = $"{dataBuff.name}({dataBuff.id})\n{dataBuff.desc}";

        ui.id = id;
        ui.SetDesc(desc);
    }
}

public class ManagerTalentPool : Singleton<ManagerTalentPool>, IManager
{
    public Dictionary<int, Talent> talentPool = new();

    private List<int> _selectorIds;
    private int _selectorIdx;

    #region Manager Func
    public void Start() { }

    public void Update() { }

    public void FixedUpdate() { }

    public void Reset() { }
    #endregion Manager Func
    public void CreateTalents()
    {
        CreateTalent(1);
        CreateTalent(2);
        CreateTalent(3);
        CreateTalent(4);
        CreateTalent(5);
        CreateTalent(6);

        _selectorIds = ManagerMonster.Instance.dictMonsters.Keys.ToList();
        _selectorIdx = 0;
    }

    private void CreateTalent(int id)
    {
        var talent = new Talent();

        var ui = GameObject.Instantiate(
            GameFlowCtrl.Instance.prefabTalentUI,
            GameFlowCtrl.Instance.rootTalentsUI);

        talent.ui = ui.GetComponent<UITalentCtrl>();
        talent.SetId(id);

        var buffDataId = Random.Range(1, 4);
        talent.SetBuffDataId(buffDataId);

        talentPool.Add(id, talent);
    }

    public void RemoveTalentById(int id)
    {
        if(talentPool.TryGetValue(id, out var talent))
        {
            GameObject.Destroy(talent.ui.gameObject);
            talentPool.Remove(id);
        }
    }

    public void OnSelectTalent(int id)
    {
        if (_selectorIds.Count == 0) return;

        var monsterId = _selectorIds[_selectorIdx];
        var monster = ManagerMonster.Instance.GetMonsterById(monsterId);

        monster.AddTalent(id);

        RemoveTalentById(id);

        _selectorIdx++;

        if (_selectorIdx >= _selectorIds.Count)
        {
            _selectorIdx = 0;
        }
    }
}
