using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace THE_LITER_KIOSK.UIManager
{
    public class UIStateManager
    {
        private List<CustomControlModel> customCtrlItems;
        public Stack<CustomControlModel> customCtrlStack;

        public UIStateManager()
        {
            InitVariables();
        }

        #region InitVariables
        private void InitVariables()
        {
            customCtrlItems = new List<CustomControlModel>();
            customCtrlStack = new Stack<CustomControlModel>();
        }
        #endregion

        public void SetCustomCtrl(CustomControlModel userCtrl, CustomControlType customCtrlType)
        {
            userCtrl.userCtrlType = customCtrlType;
            customCtrlItems.Add(userCtrl);
            userCtrl.Visibility = Visibility.Collapsed;
        }

        public void PushCustomCtrl(CustomControlModel customCtrl)
        {
            customCtrlStack.Push(customCtrl);
            SetCustomCtrlVisible(customCtrl, Visibility.Visible);
        }

        public void PopCustomCtrl()
        {
            CustomControlModel userCtrl = customCtrlStack.Peek();

            if (userCtrl != null)
            {
                SetCustomCtrlVisible(userCtrl, Visibility.Collapsed);
                customCtrlStack.Pop();
                return;
            }
        }

        public CustomControlModel GetCustomCtrl(CustomControlType customCtrlType)
        {
            return customCtrlItems.Where(x => x.userCtrlType == customCtrlType).FirstOrDefault();
        }

        public void SwitchCustomControl(CustomControlType switchCtrlTarget)
        {
            PopCustomCtrl();
            PushCustomCtrl(GetCustomCtrl(switchCtrlTarget));
        }

        private void SetCustomCtrlVisible(CustomControlModel customCtrlModel, Visibility visibility)
        {
            customCtrlModel.Visibility = visibility;
        }
    }
}
