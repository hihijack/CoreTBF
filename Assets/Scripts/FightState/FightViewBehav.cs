using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DefaultNamespace
{
    public class FightViewBehav
    {
        private PlayableDirector _director;
        
        private static FightViewBehav _inst;
        public static FightViewBehav Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new FightViewBehav();
                }

                return _inst;
            }
        }

        public void Init()
        {
            _director = Object.FindObjectOfType<PlayableDirector>();
        }

        public void Play(TimelineAsset timeLineAsset)
        {
            _director.Play(timeLineAsset);
        }
    }
}