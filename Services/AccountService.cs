using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository iAccountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            iAccountRepository = accountRepository;

        }
        public SystemAccount GetAccountById(short accountID)
        {
            return iAccountRepository.GetAccountById(accountID);
        }
        public SystemAccount GetAccountByEmail(string accountEmail)
        {
            return iAccountRepository.GetAccountByEmail(accountEmail);
        }

        public List<SystemAccount> GetAccounts()
        {
            return iAccountRepository.GetAccounts();
        }

        public void DeleteAccount(SystemAccount p)
        {
            iAccountRepository.DeleteAccount(p);
        }

        public void SaveAccount(SystemAccount p)
        {
            iAccountRepository.SaveAccount(p) ;
        }

        public void UpdateAccount(SystemAccount p)
        {
            iAccountRepository.UpdateAccount(p);
        }
    }
}
