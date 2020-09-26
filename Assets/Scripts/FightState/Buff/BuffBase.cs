
using DefaultNamespace;
using UnityEngine;

public class BuffBase 
{
    public float curDur;

    protected bool valid;
    protected BuffBaseData data;
    protected Character target;
    protected Character caster;
    protected int layer;

    float dur;

    public BuffBase(BuffBaseData data, Character target, Character caster, int layer, float dur)
    {
        this.data = data;
        this.target = target;
        this.caster = caster;
        this.layer = layer;
        this.dur = dur;
        valid = true;
    }

    public BuffBaseData GetBuffData()
    {
        return data;
    }

    public void ChangeLayer(int layerOffset)
    {
        layer += layerOffset;
        layer = Mathf.Clamp(layer, 0, data.maxLayer);
        OnChangeLayer();
        if (layer <= 0)
        {
            //移除
            valid = false;
            OnRemoved();
        }
    }

    public bool IsValid()
    {
        return valid;
    }

    public void SetLayer(int layer)
    {
        this.layer = layer;
        layer = Mathf.Clamp(layer, 0, data.maxLayer);
        OnChangeLayer();
        if (layer <= 0)
        {
            //移除
            valid = false;
            OnRemoved();
        }
    }

    public void UpdateDur(float time)
    {
        if (valid)
        {
            curDur += time;
            if (dur > 0)
            {
                if (curDur > dur)
                {
                    //设为0层,移除
                    SetLayer(0);
                }
            }
        }
    }

    public float GetDurLeft()
    {
        if (dur > 0)
        {
            return dur - curDur;
        }
        else
        {
            //永久
            return -1;
        }
    }

    public float GetDurProg()
    {
        if (dur > 0)
        {
            return curDur / dur;
        }
        else
        {
            //永久
            return -1;
        }
    }

    //刷新持续时间
    public void RestartDur() 
    {
        curDur = 0;
    }
    
    public virtual void OnAdd() { }
    protected virtual void OnChangeLayer() { }
    protected virtual void OnRemoved() { }
}
