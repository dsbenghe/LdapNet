﻿using System;
using System.Threading.Tasks;
using Novell.Directory.Ldap.NETStandard.FunctionalTests.Helpers;
using Xunit;

namespace Novell.Directory.Ldap.NETStandard.FunctionalTests
{
    public class ConnectTests
    {
        [Fact]
        public async Task Connect_Works()
        {
            await TestHelper.WithLdapConnectionAsync(
                async ldapConnection =>
                {
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                });
        }

        [Fact]
        public async Task  Connect_WithSsl_Works()
        {
            await TestHelper.WithLdapConnectionAsync(
                async ldapConnection =>
                {
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                }, true, true);
        }

        [Fact]
        public async Task Connect_WithStartTls_Works()
        {
            await TestHelper.WithLdapConnectionAsync(
                async ldapConnection =>
                {
                    try
                    {
                        await ldapConnection.StartTlsAsync();
                        await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                    }
                    finally
                    {
                        //ldapConnection.StopTls();
                    }
                }, false, true);
        }

        [Fact]
        public async Task  Connect_WithStartTlsAfterBindWithNonTls_Works()
        {
            await TestHelper.WithLdapConnectionAsync(
                async ldapConnection =>
                {
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                    await ldapConnection.StartTlsAsync();
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                    await ldapConnection.StopTlsAsync();
                }, false, true);
        }

        [Fact(Skip = "This randomly fails")]
        public async Task Connect_WithBindAfterStartTlsAndRestoreNonTls_Works()
        {
            await TestHelper.WithLdapConnectionAsync(
                async ldapConnection =>
                {
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                    await ldapConnection.StartTlsAsync();
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                    await ldapConnection.StopTlsAsync();
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                }, false, true);
        }

        [Fact]
        public async Task Connect_WithStartTls_And_Without_StopTls_Works()
        {
            await TestHelper.WithLdapConnectionAsync(
                async ldapConnection =>
                {
                    await ldapConnection.BindAsync(TestsConfig.LdapServer.RootUserDn, TestsConfig.LdapServer.RootUserPassword);
                    await ldapConnection.StartTlsAsync();
                }, false, true);
        }
    }
}
