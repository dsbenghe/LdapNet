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
// Novell.Directory.Ldap.Rfc2251.RfcCompareRequest.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.NETStandard.Asn1;

namespace Novell.Directory.Ldap.Rfc2251
{
    /// <summary>
    ///     Represents and Ldap Compare Request.
    ///     <pre>
    ///         CompareRequest ::= [APPLICATION 14] SEQUENCE {
    ///         entry           LdapDN,
    ///         ava             AttributeValueAssertion }
    ///     </pre>
    /// </summary>
    public class RfcCompareRequest : Asn1Sequence, IRfcRequest
    {
        public virtual RfcAttributeValueAssertion AttributeValueAssertion => this[1] as RfcAttributeValueAssertion;

        //*************************************************************************
        // Constructor for CompareRequest
        //*************************************************************************

        /// <summary> </summary>
        public RfcCompareRequest(RfcLdapDN entry, RfcAttributeValueAssertion ava) 
            : base(2)
        {
            Add(entry);
            Add(ava);
            if (ava.AssertionValue == null)
            {
                throw new ArgumentException("compare: Attribute must have an assertion value");
            }
        }

        /// <summary>
        ///     Constructs a new Compare Request copying from the data of
        ///     an existing request.
        /// </summary>
        internal RfcCompareRequest(Asn1Object[] origRequest, string @base)
            : base(origRequest, origRequest.Length)
        {
            // Replace the base if specified, otherwise keep original base
            if (@base != null)
            {
                this[0] =  new RfcLdapDN(@base);
            }
        }


        public override Asn1Identifier Identifier
        {
            set => base.Identifier = value;
            get => new Asn1Identifier(TagClass.APPLICATION, true, LdapMessage.COMPARE_REQUEST);
        }

        public IRfcRequest DupRequest(string @base, string filter, bool request) => new RfcCompareRequest(ToArray(), @base);

        public string RequestDN => (this[0] as RfcLdapDN).StringValue;
    }
}