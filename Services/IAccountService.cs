using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public interface IAccountService
    {
        SystemAccount GetAccountById(short accountID);
        SystemAccount GetAccountByEmail(string accountEmail);

        List<SystemAccount> GetAccounts();
        void SaveAccount(SystemAccount p);
        void DeleteAccount(SystemAccount p);
        void UpdateAccount(SystemAccount p, SystemAccount allowPasswordChange);
    }
}
