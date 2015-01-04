using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace LeagueRecorder.Windows.Caliburn
{
    public class FlyoutReactiveScreen : ReactiveScreen
    {
        #region Fields
        private bool _isOpen;
        private Position _position;
        #endregion

        #region Properties
        public bool IsOpen
        {
            get { return this._isOpen; }
            set
            {
                this.RaiseAndSetIfChanged(ref this._isOpen, value);

                if (this.IsOpen)
                {
                    if (this.IsActive == false)
                    {
                        ((IActivate) this).Activate();
                    }
                }
                else
                {
                    if (this.IsActive)
                    {
                        ((IDeactivate) this).Deactivate(false);
                    }
                }
            }
        }
        public Position Position
        {
            get { return this._position; }
            set { this.RaiseAndSetIfChanged(ref this._position, value); }
        }
        #endregion

        #region Methods
        public bool TryHide()
        {
            if (this.IsOpen == false)
                return false;

            this.IsOpen = false;
            return true;
        }

        public bool TryShow()
        {
            if (this.IsOpen)
                return false;

            this.IsOpen = true;
            return true;
        }

        public void Toggle()
        {
            if (this.TryShow() == false)
                this.TryHide();
        }
        #endregion
    }
}