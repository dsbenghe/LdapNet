/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
*
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to  permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/

//
// Novell.Directory.Ldap.Extensions.AddReplicaRequest.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using System.IO;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.Utilclass;

namespace Novell.Directory.Ldap.Extensions
{
    /// <summary>
    ///     Adds a replica to the specified directory server.
    ///     To add a replica to a particular server, you must create an instance of
    ///     this class and then call the extendedOperation method with this
    ///     object as the required LdapExtendedOperation parameter.
    ///     The addReplicaRequest extension uses the following OID:
    ///     2.16.840.1.113719.1.27.100.7
    ///     The requestValue has the following format:
    ///     requestValue ::=
    ///     flags       INTEGER
    ///     replicaType INTEGER
    ///     serverName  LdapDN
    ///     dn          LdapDN.
    /// </summary>
    public class AddReplicaRequest : LdapExtendedOperation
    {
        /// <summary>
        ///     Constructs a new extended operation object for adding a replica to the
        ///     specified server.
        /// </summary>
        /// <param name="dn">
        ///     The distinguished name of the replica's partition root.
        /// </param>
        /// <param name="serverDn">
        ///     The server on which the new replica will be added.
        /// </param>
        /// <param name="replicaType">
        ///     The type of replica to add. The replica
        ///     types are defined in the ReplicationConstants class.
        /// </param>
        /// <param name="flags">
        ///     Specifies whether all servers in the replica ring must be up
        ///     before proceeding. When set to zero, the status of the servers is not
        ///     checked. When set to Ldap_ENSURE_SERVERS_UP, all servers must be up for the
        ///     operation to proceed.
        /// </param>
        /// <exception>
        ///     LdapException A general exception which includes an error message
        ///     and an Ldap error code.
        /// </exception>
        /// <seealso cref="ReplicationConstants.LdapRtMaster">
        /// </seealso>
        /// <seealso cref="ReplicationConstants.LdapRtSecondary">
        /// </seealso>
        /// <seealso cref="ReplicationConstants.LdapRtReadonly">
        /// </seealso>
        /// <seealso cref="ReplicationConstants.LdapRtSubref">
        /// </seealso>
        /// <seealso cref="ReplicationConstants.LdapRtSparseWrite">
        /// </seealso>
        /// <seealso cref="ReplicationConstants.LdapRtSparseRead">
        /// </seealso>
        public AddReplicaRequest(string dn, string serverDn, int replicaType, int flags)
            : base(ReplicationConstants.AddReplicaReq, null)
        {
            try
            {
                if ((object)dn == null || (object)serverDn == null)
                {
                    throw new ArgumentException(ExceptionMessages.ParamError);
                }

                var encodedData = new MemoryStream();
                var encoder = new LberEncoder();

                var asn1Flags = new Asn1Integer(flags);
                var asn1ReplicaType = new Asn1Integer(replicaType);
                var asn1ServerDn = new Asn1OctetString(serverDn);
                var asn1Dn = new Asn1OctetString(dn);

                asn1Flags.Encode(encoder, encodedData);
                asn1ReplicaType.Encode(encoder, encodedData);
                asn1ServerDn.Encode(encoder, encodedData);
                asn1Dn.Encode(encoder, encodedData);

                SetValue(encodedData.ToArray());
            }
            catch (IOException ioe)
            {
                throw new LdapException(ExceptionMessages.EncodingError, LdapResultCode.EncodingError, null, ioe);
            }
        }
    }
}