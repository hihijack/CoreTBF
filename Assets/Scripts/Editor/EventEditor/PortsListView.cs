using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UnityEngine.UIElements 
{
    public class PortsListView : ListView
    {
        EventNodeView _nodeSource;
        public PortsListView()
        {
            this.bindItem = BindItem;
            this.makeItem = MakeItem;
            this.itemHeight = 45;
        }

        public void SetDataSource(EventNodeView node)
        {
            this._nodeSource = node;
            this.itemsSource = _nodeSource.GetSortedProtsLst();
        }

        private VisualElement MakeItem()
        {
            ItemPortInfo itemPortInfo = new ItemPortInfo(_nodeSource, this);
            return itemPortInfo;
        }

        private void BindItem(VisualElement element, int index)
        {
            var itemPortInfo = element as ItemPortInfo;
            itemPortInfo.SetData(_nodeSource.GetSortedProtsLst()[index]);
            itemPortInfo.Refresh();
        }

        public void SortWithIndex()
        {
            Sort(SortByIndex);
            Refresh();
        }

        private int SortByIndex(VisualElement x, VisualElement y)
        {
            var ipiX = x as ItemPortInfo;
            var ipiY = y as ItemPortInfo;
            return ipiX.data.index - ipiY.data.index;
        }

        internal void OnDelete(ItemPortInfo itemPortInfo)
        {
            this.Remove(itemPortInfo);
        }
    }
}

