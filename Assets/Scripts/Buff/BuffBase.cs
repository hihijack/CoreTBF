
using DefaultNamespace;
using UnityEngine;

public class BuffBase 
{
    public float dur;

    protected bool valid;
    protected BuffData data;
    protected Character target;
    protected Character caster;
    protected int layer;


    public BuffBase(BuffData data, Character target, Character caster, int layer)
    {
        this.data = data;
        this.target = target;
        this.caster = caster;
        this.layer = layer;
        valid = true;
    }

    public BuffData GetBuffData()
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
            dur += time;
            if (data.dur > 0)
            {
                if (dur > data.dur)
                {
                    //设为0层,移除
                    SetLayer(0);
                }
            }
        }
    }

    public float GetDurLeft()
    {
        if (data.dur > 0)
        {
            return data.dur - dur;
        }
        else
        {
            //永久
            return -1;
        }
    }

    public float GetDurProg()
    {
        if (data.dur > 0)
        {
            return dur / data.dur;
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
        dur = 0;
    }
    
    public virtual void OnAdd() { }
    protected virtual void OnChangeLayer() { }
    protected virtual void OnRemoved() { }
}
