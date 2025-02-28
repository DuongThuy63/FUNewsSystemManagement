using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository
    {
         void SaveAccount(SystemAccount p);
        void DeleteAccount(SystemAccount p);
        void UpdateAccount(SystemAccount p, SystemAccount allowPasswordChange);
        SystemAccount GetAccountById(short accountID);

        SystemAccount GetAccountByEmail(string accountEmail);
        List<SystemAccount> GetAccounts();
    }
}
