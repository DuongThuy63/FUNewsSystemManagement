using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        
        public static SystemAccount GetAccountById(short accountID)
        {
            using var db = new FunewsManagementContext();
            return db.SystemAccounts.FirstOrDefault(c => c.AccountId.Equals(accountID));
        }

        public static SystemAccount GetAccountByEmail (string accountEmail)
        {
            using var db = new FunewsManagementContext();
            return db.SystemAccounts.FirstOrDefault(c => c.AccountEmail.Equals(accountEmail));
        }

        public static List<SystemAccount> GetAccounts()
        {
            var listAccounts = new List<SystemAccount>();
            try
            {
                using var db = new FunewsManagementContext();
                listAccounts = db.SystemAccounts.ToList();
            }
            catch (Exception e) { }
            return listAccounts;
        }

        public static void UpdateAccount(SystemAccount p, SystemAccount loggedInUser)
        {
            try
            {
                using var context = new FunewsManagementContext();
                var existingAccount = context.SystemAccounts.FirstOrDefault(a => a.AccountId == p.AccountId);

                if (existingAccount == null)
                {
                    throw new Exception("Tài khoản không tồn tại!");
                }

                // 🔹 Admin (Role 1) có thể chỉnh sửa thông tin, nhưng không thay đổi mật khẩu
                if (loggedInUser.AccountRole == 1)
                {
                    p.AccountPassword = existingAccount.AccountPassword; // Giữ nguyên mật khẩu
                }

                // 🔹 Lecturer (Role 2) chỉ được thay đổi mật khẩu của chính họ
                if (loggedInUser.AccountRole == 2 && loggedInUser.AccountId == p.AccountId)
                {
                    existingAccount.AccountPassword = p.AccountPassword;
                }

                // Cập nhật các thông tin khác

                existingAccount.AccountName = p.AccountName;
                existingAccount.AccountEmail = p.AccountEmail;
                existingAccount.AccountRole = p.AccountRole;
                context.SystemAccounts.Update(existingAccount);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi khi cập nhật tài khoản: {e.Message}");
                throw;
            }
        }


        public static void SaveAccount(SystemAccount p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.SystemAccounts.Add(p);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi khi lưu account: {e.Message}");
                throw;
            }
        }

        public static void DeleteAccount(SystemAccount p)
        {
            try
            {
                using var context = new FunewsManagementContext();
                var p1 =
                    context.SystemAccounts.SingleOrDefault(c => c.AccountId == p.AccountId);
                context.SystemAccounts.Remove(p1);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
