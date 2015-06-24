using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.TeamFoundation.MVVM;
using System.Windows.Input;
using SQLMaker2.Properties;
using System.Windows;

namespace SQLMaker2
{
    class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        // start: IDataErrorInfo interfaces
        private readonly Dictionary<string, string>
            _errors = new Dictionary<string, string>();

        public string Error
        {
            get
            {
                return _errors.Count > 0 ?
                    "項目の値が不正です" : "";
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return _errors.ContainsKey(propertyName) ?
                    _errors[propertyName] : "";
            }
        }

        protected void UpdateErrors(string propertyName, string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                _errors.Remove(propertyName);
            }
            else
            {
                _errors[propertyName] = errorMessage;
            }
        }
        // End:  IDataErrorInfo

        // start: ViewModelBase interfaces
        public bool HasErrors
        {
            get
            {
                return _errors.Count != 0 || !string.IsNullOrEmpty(Error);
            }
        }
        // End: ViewModelBase interfaces

        private string _errorMessage = "";
        public string errorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if (value == "")
                {
                    _errorMessage = "";
                    return;
                }
                _errorMessage += value; 
            }
        }

        private bool checkIPaddr(string isIPaddr)
        {
            bool result = System.Text.RegularExpressions.Regex.IsMatch(
                isIPaddr,
                @"^\d{1,3}[.]\d{1,3}[.]\d{1,3}[.]\d{1,3}$"
                );
            return result;
        }

        private bool checkUser(string isUser)
        {
            bool result = System.Text.RegularExpressions.Regex.IsMatch(
                isUser,
                @"^[a-zA-Z0-9]+$"
                );
            return result;
        }

        private bool checkArgs()
        {
            bool result = true;
            if (Router == "" || Router == null )
            {
                errorMessage = "「収容ルータ」が空です。\n";
                result = false;
            }
            if (routerIsNotResistered && (IPaddr == "" || IPaddr == null))
            {
                errorMessage = "「IPアドレス」が空です。\n";
                result = false;
            }
            else if (routerIsNotResistered && !checkIPaddr(IPaddr))
            {
                errorMessage = "「IPアドレス」の入力形式に誤りがあります。\n";
                result = false;
            }
            if (User == "" || User == null)
            {
                errorMessage = "「ユーザ名」が空です。\n";
                result = false;
            }
            else if (!checkUser(User))
            {
                errorMessage = "「ユーザ名」に使える文字は英数字(「a-z」「A-Z」「0-9」)です。\n";
                result = false;
            }
            if (NWID == "" || NWID == null)
            {
                errorMessage = "「ネットワークID」が空です。\n";
                result = false;
            }
            if (IntNo == "" || IntNo == null)
            {
                errorMessage = "「インターフェースNo」が空です。\n";
                result = false;
            }
            else
            {
                try
                {
                    int numTest = int.Parse(IntNo);
                    if (numTest < 1) numTest = 1;
                    if (numTest > 2147483647) numTest = 2147483647;
                    IntNo = numTest.ToString();
                }
                catch
                {
                    IntNo = "";
                    errorMessage = "「インターフェースNo」は数字で入力してください。\n";
                    result = false;
                }

            }
            if (Ap == null || Ap.ApCode == "")
            {
                errorMessage = "「AP」が空です。\n";
                result = false;
            }
            if (Zone == "" || Zone == null)
            {
                errorMessage = "「ゾーンID」が空です。\n";
                result = false;
            }
            return result;
        }

        public void Dialog_Cautions(string messages)
        {
            string messageBoxText = messages;
            string caption = "入力エラー";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;

            MessageBox.Show(messageBoxText, caption, button, icon);
            errorMessage = "";
            SQL = "";
        }

        private void setSQL()
        {
            string nl = System.Environment.NewLine;
            string sql = "";
            
            if (checkArgs() == false)
            {
                Dialog_Cautions(errorMessage);
                return;
            }
            if (routerIsNotResistered) 
                sql += string.Format("INSERT INTO m_sw VALUES('{0}', '{1}', '{2}', 0);{3}",
                                      Router, IPaddr, Settings.Default.CName, nl);
            if (userIsNotResistered)
                sql += string.Format("INSERT INTO m_user VALUES('{0}');{1}",
                                      User,nl);
            if (vportIsNotResistered)
                sql += string.Format("INSERT INTO m_vport VALUES('{0}', '{1}', {2});{3}",
                                      User, Ap.ApCode, Zone, nl);
            if (chargeIsNotResistered)
            {
            //    sql += string.Format("INSERT INTO m_inf VALUES('{0}', {1});{2}",
            //                          Router, IntNo, nl);
                sql += string.Format("INSERT INTO m_charge VALUES('{0}', {1}, '{2}', '{3}', {4}, '{5}');{6}",
                                      Router, IntNo, User, Ap.ApCode, Zone, NWID, nl);
            }
            SQL = sql;
        }

        private void resetSQL()
        {
            SQL = "";
            Router = "";
            IPaddr = "";
            User = "";
            NWID = "";
            IntNo = "";
            Ap = Aps.Items[0];
            Zone = Zones.Items[0];
            routerIsNotResistered = userIsNotResistered
                = vportIsNotResistered = chargeIsNotResistered = true;
        }

        private void ExecuteGeneSQLCommand()
        {
            //todo
            setSQL();
        }
        private void ExecuteResetCommand()
        {
            //todo
            resetSQL();
        }

        private ICommand _GeneSQLCommand;
        public ICommand GeneSQLCommand
        {
            get
            {
                if ( _GeneSQLCommand == null )
                {
                    _GeneSQLCommand = new RelayCommand(ExecuteGeneSQLCommand);
                }
                return _GeneSQLCommand;
            }
        }

        private ICommand _ResetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (_ResetCommand == null)
                {
                    _ResetCommand = new RelayCommand(ExecuteResetCommand);
                }
                return _ResetCommand;
            }
        }

        public string Zone
        {
            get
            {
                return Zones.Item;
            }
            set
            {
                if (Zones.Item == value) return;
                Zones.Item = value;
                RaisePropertyChanged("Zone");
            }
        }

        public Ap Ap
        {
            get
            {
                return Aps.Item;
            }
            set
            {
                if (Aps.Item == value) return;
                Aps.Item = value;
                RaisePropertyChanged("Ap");
            }
        }

        // ラジオボタンに対応する
        private bool _routerIsNotResistered = true;
        public bool routerIsNotResistered
        {
            get
            {
                return _routerIsNotResistered;
            }
            set
            {
                if (_routerIsNotResistered == value) return;
                _routerIsNotResistered = value;
                RaisePropertyChanged("routerIsNotResistered");
            }
        }

        private bool _userIsNotResistered = true;
        public bool userIsNotResistered
        {
            get
            {
                return _userIsNotResistered;
            }
            set
            {
                if (_userIsNotResistered == value) return;
                _userIsNotResistered = value;
                RaisePropertyChanged("userIsNotResistered");
            }
        }

        private bool _vportIsNotResistered = true;
        public bool vportIsNotResistered
        {
            get
            {
                return _vportIsNotResistered;
            }
            set
            {
                if (_vportIsNotResistered == value) return;
                _vportIsNotResistered = value;
                RaisePropertyChanged("vportIsNotResistered");
            }
        }

        private bool _chargeIsNotResistered = true;
        public bool chargeIsNotResistered
        {
            get
            {
                return _chargeIsNotResistered;
            }
            set
            {
                if (_chargeIsNotResistered == value) return;
                _chargeIsNotResistered = value;
                RaisePropertyChanged("chargeIsNotResistered");
            }
        }

        // 
        private string _Router;
        public string Router
        {
            get
            {
                return _Router;
            }
            set
            {
                if (_Router == value) return;
                _Router = value;
                RaisePropertyChanged("Router");
            }
        }

        private string _IPaddr;
        public string IPaddr
        {
            get
            {
                return _IPaddr;
            }
            set
            {
                if (_IPaddr == value) return;
                _IPaddr = value;
                RaisePropertyChanged("IPaddr");
            }
        }

        private string _User;
        public string User
        {
            get
            {
                return _User;
            }
            set
            {
                if (_User == value) return;
                _User = value;
                RaisePropertyChanged("User");
            }
        }

        private string _NWID;
        public string NWID
        {
            get
            {
                return _NWID;
            }
            set
            {
                if (_NWID == value) return;
                _NWID = value;
                RaisePropertyChanged("NWID");
            }
        }

        private string _IntNo;
        public string IntNo
        {
            get
            {
                return _IntNo;
            }
            set
            {
                if (_IntNo == value) return;
                _IntNo = value;
                RaisePropertyChanged("IntNo");
            }
        }

        private string _SQL;
        public string SQL
        {
            get
            {
                return _SQL;
            }
            set
            {
                if (_SQL == value) return;
                _SQL = value;
                RaisePropertyChanged("SQL");
            }
        }
    }
}
