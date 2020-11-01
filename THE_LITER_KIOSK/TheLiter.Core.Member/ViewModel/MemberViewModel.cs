using Prism.Commands;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Input;
using TheLiter.Core.DBManager;
using TheLiter.Core.Member.Model;

namespace TheLiter.Core.Member.ViewModel
{
    public class MemberViewModel : MySqlDBConnectionManager, INotifyPropertyChanged
    {
        private DBManager<MemberModel> memberDBManager = new DBManager<MemberModel>();

        public delegate void OnLoginResultRecievedHandler(object sender, bool success);
        public event OnLoginResultRecievedHandler OnLoginResultRecieved;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties
        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        private string _pw;
        public string Pw
        {
            get => _pw;
            set
            {
                _pw = value;
                NotifyPropertyChanged(nameof(Pw));
            }
        }

        private string _qrCode;
        public string QrCode
        {
            get => _qrCode;
            set
            {
                _qrCode = value;
                NotifyPropertyChanged(nameof(QrCode));
            }
        }

        private string _barCode;
        public string BarCode
        {
            get => _barCode;
            set
            {
                _barCode = value;
                NotifyPropertyChanged(nameof(BarCode));
            }
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set
            {
                _serverAddress = value.Trim();
                NotifyPropertyChanged(nameof(ServerAddress));
            }
        }

        private bool _btnEnabled;
        public bool BtnEnabled
        {
            get => _btnEnabled;
            set
            {
                _btnEnabled = value;
                NotifyPropertyChanged(nameof(BtnEnabled));
            }
        }
        #endregion

        #region Command
        public ICommand SignUpCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Constructor
        public MemberViewModel()
        {
            InitCommands();
        }
        #endregion

        #region Init
        private void InitCommands()
        {
            SignUpCommand = new DelegateCommand(OnSignUp, CanSignUp).ObservesProperty(() => BarCode);
            LoginCommand = new DelegateCommand(OnLogin, CanLogin).ObservesProperty(() => Pw);
        }

        private void ClearSignUpData()
        {
            Id = string.Empty;
            Pw = string.Empty;
            QrCode = string.Empty;
            BarCode = string.Empty;
        }

        private void ClearLoginData()
        {
            Id = string.Empty;
            Pw = string.Empty;
        }
        #endregion

        #region Command Method
        private bool CanSignUp()
        {
            return (Id != null) && (Pw != null) && (QrCode != null) && (BarCode != null);
        }

        private async void OnSignUp()
        {
            try
            {
                using (IDbConnection db = GetConnection())
                {
                    db.Open();

                    var memberModel = new MemberModel();
                    memberModel.BarCode = BarCode;
                    memberModel.QrCode = QrCode;
                    memberModel.Id = Id;
                    memberModel.Pw = Pw;

                    string insertSql = @"
INSERT INTO member_tb(
    BarCode,
    QrCode,
    Id,
    Pw
)
VALUES(
    @qrCode,
    @barCode,
    @id,
    @pw
);";
                    if (await memberDBManager.InsertAsync(db, insertSql, memberModel) == 1)
                    {
                        Debug.WriteLine("SUCCESS SIGN UP");
                        ClearSignUpData();
                    }
                    else
                    {
                        Debug.WriteLine("FAILURE SIGN UP");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write("SIGN UP ERROR : " + e.Message);
            }
        }

        internal async void OnLogin()
        {
            BtnEnabled = false;
            var member = new MemberModel();

            try
            {
                using (IDbConnection db = GetConnection())
                {
                    db.Open();

                    var memberModel = new MemberModel();

                    string selectSql = $@"
SELECT
    *
FROM
     member_tb
WHERE
    id = '{Id}'
AND
    pw = '{Pw}'
;";
                    member = await memberDBManager.GetSingleDataAsync(db, selectSql, "");
                    if (member.Id == null)
                        member = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("LOGIN ERROR : " + e.Message);
            }

            if (member != null)
            {
                SendOnLoginResultRecievedEvent(true);
            }
            else
            {
                SendOnLoginResultRecievedEvent(false);
            }

            BtnEnabled = true;
        }

        private bool CanLogin()
        {
            return (Id != null) && (Pw != null);
        }
        #endregion

        private void SendOnLoginResultRecievedEvent(bool success)
        {
            OnLoginResultRecieved?.Invoke(this, success);
        }
    }
}
