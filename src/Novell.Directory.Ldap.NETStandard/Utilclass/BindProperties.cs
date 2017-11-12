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
// Novell.Directory.Ldap.Utilclass.BindProperties.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System.Collections;

namespace Novell.Directory.Ldap.Utilclass
{
    /// <summary> Encapsulates an Ldap Bind properties</summary>
    public class BindProperties
    {
        /// <summary> gets the protocol version</summary>
        public virtual int ProtocolVersion { get; }

        /// <summary>
        ///     Gets the authentication dn
        /// </summary>
        /// <returns>
        ///     the authentication dn for this connection
        /// </returns>
        public virtual string AuthenticationDN { get; }

        /// <summary>
        ///     Gets the authentication method
        /// </summary>
        /// <returns>
        ///     the authentication method for this connection
        /// </returns>
        public virtual string AuthenticationMethod { get; }

        /// <summary>
        ///     Gets the SASL Bind properties
        /// </summary>
        /// <returns>
        ///     the sasl bind properties for this connection
        /// </returns>
        public virtual Hashtable SaslBindProperties { get; }

        /// <summary>
        ///     Gets the SASL callback handler
        /// </summary>
        /// <returns>
        ///     the sasl callback handler for this connection
        /// </returns>
        public virtual object SaslCallbackHandler { get; }

        /// <summary>
        ///     Indicates whether or not the bind properties specify an anonymous bind
        /// </summary>
        /// <returns>
        ///     true if the bind properties specify an anonymous bind
        /// </returns>
        public virtual bool Anonymous { get; }


        public BindProperties(int version, string dn, string method, bool anonymous, Hashtable bindProperties,
            object bindCallbackHandler)
        {
            ProtocolVersion = version;
            AuthenticationDN = dn;
            AuthenticationMethod = method;
            Anonymous = anonymous;
            SaslBindProperties = bindProperties;
            SaslCallbackHandler = bindCallbackHandler;
        }
    }
}