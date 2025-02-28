using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public SystemAccount GetAccountById(short accountID)
            => SystemAccountDAO.GetAccountById(accountID);

        public SystemAccount GetAccountByEmail(string accountEmail)
            => SystemAccountDAO.GetAccountByEmail(accountEmail);

        public List<SystemAccount> GetAccounts() => SystemAccountDAO.GetAccounts();

        public void UpdateAccount(SystemAccount p, SystemAccount allowPasswordChange)
        => SystemAccountDAO.UpdateAccount(p, allowPasswordChange);


        public void DeleteAccount(SystemAccount p) => SystemAccountDAO.DeleteAccount(p);
        public void SaveAccount(SystemAccount p) => SystemAccountDAO.SaveAccount(p);
        
    }
}
