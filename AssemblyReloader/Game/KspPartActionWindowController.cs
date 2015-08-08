//using System.Collections.Generic;

//namespace AssemblyReloader.Game
//{
//    public class KspPartActionWindowController : IPartActionWindowController
//    {
//        private readonly List<UIPartActionWindow> _windows = new List<UIPartActionWindow>();
 
//        public void Close()
//        {
//            UIPartActionController.Instance.Deselect(true);
//        }

//        public void Add(UIPartActionWindow window)
//        {
//            if (!_windows.Contains(window)) _windows.Add(window);
//        }

//        public void Remove(UIPartActionWindow window)
//        {
//            if (_windows.Contains(window)) _windows.Remove(window);
//        }

//        public void Refresh()
//        {
//            _windows.ForEach(w => w.displayDirty = true);
//        }
//    }
//}
