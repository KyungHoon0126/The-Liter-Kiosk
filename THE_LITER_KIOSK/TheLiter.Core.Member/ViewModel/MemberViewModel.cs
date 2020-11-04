using Prism.Commands;
using System;
using System.ComponentModel;
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

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
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
            Name = string.Empty;
            QrCode = string.Empty;
            BarCode = string.Empty;
        }
        #endregion

        #region Command Method
        private bool CanSignUp()
        {
            return (Id != null) && (Pw != null) && (Name != null) && (QrCode != null) && (BarCode != null);
        }

        private async void OnSignUp()
        {
            try
            {
                using (var db = GetConnection())
                {
                    db.Open();

                    var memberModel = new MemberModel();
                    memberModel.BarCode = BarCode;
                    memberModel.QrCode = QrCode;
                    memberModel.Id = Id;
                    memberModel.Pw = Pw;
                    memberModel.Name = Name;

                    string insertSql = @"
INSERT INTO member_tb(
    BarCode,
    QrCode,
    Id,
    Pw,
    Name
)
VALUES(
    @qrCode,
    @barCode,
    @id,
    @pw,
    @name
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
                using (var db = GetConnection())
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
                SendOnLoginResultRecievedEvent(false);
                return;
            }

            if (member.Name != null)
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

        #region DataBase
        internal async void GetMemberData()
        {
            try
            {
                using (var db = GetConnection())
                {
                    db.Open();

                    string selectSql = $@"
SELECT
    *
FROM
    member_tb
WHERE
    id = '{Id}'
";
                    var member = await memberDBManager.GetSingleDataAsync(db, selectSql, "");

                    if (member != null)
                    {
                        Name = member.Name;
                        QrCode = member.QrCode;
                        BarCode = member.BarCode;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write("GET MEMBER DATA ERROR : " + e.Message);
            }
        }
        #endregion

        private void SendOnLoginResultRecievedEvent(bool success)
        {
            OnLoginResultRecieved?.Invoke(this, success);
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
