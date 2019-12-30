using Microsoft.AspNetCore.Identity;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using MyMedia.Core.User;
using Dapper;


namespace MyMedia.Core.User
{
    
    public class ProfielUserStore : IUserStore<Profiel>,IUserPasswordStore<Profiel>
    {


        public void Dispose()
        {
        }
        public Task<string> GetUserIdAsync(Profiel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public async Task<Profiel> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Profiel>(
                     "select * From Profiel" +
                     "where [Id] = @id",
                     new
                     {
                         id = userId
                     });
               
            }
            
        }

        public async Task<Profiel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Profiel>(
                     "SELECT * FROM Profiel WHERE NormalizedUserName = @name",
                     new
                     {
                         name = normalizedUserName
                     });

            }
        }

        public Task<string> GetNormalizedUserNameAsync(Profiel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(Profiel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        

        public Task<string> GetUserNameAsync(Profiel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(Profiel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(Profiel user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Profiel user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        
        public Task SetUserNameAsync(Profiel user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Profiel user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update Profiel"+
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
        public async Task<IdentityResult> CreateAsync(Profiel user, CancellationToken cancellationToken)
        {
            // orm dapper
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "insert into Profiel([Id]," +
                    "[UserName]," +
                    "[NormalizedUserName]," +
                    "[PasswordHash])" +
                    "Values(@id,@username,@normalizedUserName,@passwordHash)",
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
        public Task<IdentityResult> DeleteAsync(Profiel user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}