﻿using System;
using System.Threading.Tasks;
using Household.DataBaseModels;

namespace HouseholdTests.Infrastructure
{
    public class TestEnvironment
    {
        private readonly TestWebApplicationFactory factory;
        private readonly TestAuthorization authorization;

        public TestEnvironment()
        {
            factory = new TestWebApplicationFactory();
            authorization = new TestAuthorization(factory);
        }

        public async Task<TestUser> RegisterNewUser(Role role = Role.Cook)
        {
            var user = new TestUser
            {
                Login = $"{Guid.NewGuid().ToString().Substring(10)}@mail.ru",
                Password = "testPass"
            };

            await authorization.Register(user, role);

            await authorization.Login(user);

            await authorization.RequireIdentityServerToken(user.Client);

            return user;
        }
    }
}
