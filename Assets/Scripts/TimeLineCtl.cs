using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineCtl
{

    PlayableDirector _director;

    public TimeLineCtl(PlayableDirector director) 
    {
        _director = director;
    }

    public void Play(TimelineAsset timelineAsset) 
    {
        _director.Play(timelineAsset);
    }
}
