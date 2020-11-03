using Prism.Mvvm;
using System;

namespace TheLiter.Core.Member.Model
{
    public class MemberModel : BindableBase, ICloneable
    {
        private int _idx;
        public int Idx
        {
            get => _idx;
            set => SetProperty(ref _idx, value);
        }

        private string _qrCode;
        public string QrCode
        {
            get => _qrCode;
            set => SetProperty(ref _qrCode, value);
        }

        private string _barCode;
        public string BarCode
        {
            get => _barCode;
            set => SetProperty(ref _barCode, value);
        }

        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _pw;
        public string Pw
        {
            get => _pw;
            set => SetProperty(ref _pw, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public object Clone()
        {
            return new MemberModel()
            {
                Idx = this.Idx,
                QrCode = this.QrCode,
                BarCode = this.BarCode,
                Id = this.Id,
                Pw = this.Pw,
                Name = this.Name
            };
        }
    }
}
