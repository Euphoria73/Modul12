using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Modul11_UI_HW.EventHandlers
{
    /// <summary>
    /// Класс, определяющий поведение TreeView
    /// </summary>
    public class TreeViewItemBehavior
    {
        //создаём свойство зависимости для связки данных модели к представлению
        public static readonly DependencyProperty BringIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached("BringIntoViewWhenSelected", typeof(bool), 
                                                 typeof(TreeViewItemBehavior), new UIPropertyMetadata(false, OnBringIntoViewWhenSelectedChanged));                                                     

        public static bool GetBringIntoViewWhenSelected(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(BringIntoViewWhenSelectedProperty);
        }

        public static void SetBringIntoViewWhenSelected(TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(BringIntoViewWhenSelectedProperty, value);
        }
               
        static void OnBringIntoViewWhenSelectedChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (!(depObj is TreeViewItem item))
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.BringIntoView();
        }
    }
}
