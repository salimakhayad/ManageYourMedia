using Microsoft.AspNetCore.Identity;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using MyMedia.Core.User;
using Dapper;
using System.Data;

namespace MyMedia.Core.User
{
    
    public class MediaUserStore : IUserStore<MediaUser>,IUserPasswordStore<MediaUser>
    {


        public void Dispose()
        {
        }
        public Task<string> GetUserIdAsync(MediaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public async Task<MediaUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MediaUser>(
                     "select * From AspNetUsers" +
                     "where [Id] = @id",
                     new
                     {
                         id = userId,
                     });
               
            }
            
        }

        public async Task<MediaUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MediaUser>(
                     "SELECT * FROM AspNetUsers WHERE NormalizedUserName = @name",
                     new
                     {
                         name = normalizedUserName
                     });

            }
        }

        public Task<string> GetNormalizedUserNameAsync(MediaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(MediaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        

        public Task<string> GetUserNameAsync(MediaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(MediaUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(MediaUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(MediaUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        
        public Task SetUserNameAsync(MediaUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MediaUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update AspNetUsers" +
                    "set [Id] = @id," +
                    "[UserName] = @username," +
                    "[NormalizedUserName] = @normalizedUserName," +
                    "[PasswordHash] = @passwordHash" +
                    "where [Id] = @id",
                    new
                    {
                        id = user.Id,
                        username = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    }
                );
            }
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> CreateAsync(MediaUser user, CancellationToken cancellationToken)
        {
            // orm dapper
            string commandText = "insert into AspNetUsers([Id],[UserName],[NormalizedUserName],[PasswordHash]," +
                                "[EmailConfirmed],[PhoneNumberConfirmed],[TwoFactorEnabled]," +
                                "[LockoutEnabled],[AccessFailedCount])" +
                    "Values(@id,@username,@normalizedUserName," +
                            "@passwordHash,@emailConfirmed," +
                            "@phoneNumberConfirmed,@twoFactorEnabled," +
                            "@lockoutEnabled,@accessFailedCount)";
           
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(commandText,
                    new
                    {
                        id = user.Id,
                        username = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash,
                        emailConfirmed = false,
                        phoneNumberConfirmed = false,
                        twoFactorEnabled = false,
                        lockoutEnabled = 0,
                        accessFailedCount = 0
                    });
            }
            return IdentityResult.Success;

        }
        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;" +
                                                "Database=MediaDb;" +
                                                "Trusted_Connection=True;" +
                                                "MultipleActiveResultSets=true");
            connection.Open();
            return connection;
            // consider using IDisposable

        }
        public Task<IdentityResult> DeleteAsync(MediaUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}