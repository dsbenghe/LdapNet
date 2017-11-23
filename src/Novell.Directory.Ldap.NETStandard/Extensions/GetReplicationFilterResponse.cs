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
// Novell.Directory.Ldap.Extensions.GetReplicationFilterResponse.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System.IO;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.Rfc2251;

namespace Novell.Directory.Ldap.Extensions
{
    /// <summary>
    ///     This object represent the filter returned fom a GetReplicationFilterRequest.
    ///     An object in this class is generated from an ExtendedResponse object
    ///     using the ExtendedResponseFactory class.
    ///     The GetReplicationFilterResponse extension uses the following OID:
    ///     2.16.840.1.113719.1.27.100.38
    /// </summary>
    public class GetReplicationFilterResponse : LdapExtendedResponse
    {
        /// <summary>
        ///     Returns the replicationFilter as an array of classname-attribute name pairs
        /// </summary>
        /// <returns>
        ///     String array contining a two dimensional array of strings.  The first
        ///     element of each array is the class name the others are the attribute names
        /// </returns>
        public virtual string[][] ReplicationFilter { get; private set; }

        /// <summary>
        ///     Constructs an object from the responseValue which contains the replication
        ///     filter.
        ///     The constructor parses the responseValue which has the following
        ///     format:
        ///     responseValue ::=
        ///     SEQUENCE of SEQUENCE {
        ///     classname  OCTET STRING
        ///     SEQUENCE of ATTRIBUTES
        ///     }
        ///     where
        ///     ATTRIBUTES:: OCTET STRING
        /// </summary>
        /// <exception>
        ///     IOException The responseValue could not be decoded.
        /// </exception>
        public GetReplicationFilterResponse(RfcLdapMessage rfcMessage) : base(rfcMessage)
        {
            if (ResultCode != LdapException.SUCCESS)
            {
                ReplicationFilter = new string[0][];
                for (var i = 0; i < 0; i++)
                {
                    ReplicationFilter[i] = new string[0];
                }
            }
            else
            {
                // parse the contents of the reply
                var returnedValue = Value;
                if (returnedValue == null)
                    throw new IOException("No returned value");

                // Create a decoder object
                var decoder = new LBERDecoder();

                // We should get back a sequence
                var returnedSequence = decoder.Decode(returnedValue) as Asn1Sequence;
                if (returnedSequence == null)
                    throw new IOException("Decoding error");

                // How many sequences in this list
                var numberOfSequences = returnedSequence.Count;
                ReplicationFilter = new string[numberOfSequences][];

                // Parse each returned sequence object
                for (var classNumber = 0; classNumber < numberOfSequences; classNumber++)
                {
                    // Get the next Asn1Sequence
                    var asn1_innerSequence = returnedSequence[classNumber] as Asn1Sequence;
                    if (asn1_innerSequence == null)
                        throw new IOException("Decoding error");

                    // Get the asn1 encoded classname
                    var asn1_className = asn1_innerSequence[0] as Asn1OctetString;
                    if (asn1_className == null)
                        return;

                    // Get the attribute List
                    var asn1_attributeList = asn1_innerSequence[1] as Asn1Sequence;
                    if (asn1_attributeList == null)
                        throw new IOException("Decoding error");

                    var numberOfAttributes = asn1_attributeList.Count;
                    ReplicationFilter[classNumber] = new string[numberOfAttributes + 1];

                    // Get the classname
                    ReplicationFilter[classNumber][0] = asn1_className.StringValue;
                    if (ReplicationFilter[classNumber][0] == null)
                        throw new IOException("Decoding error");

                    for (var attributeNumber = 0; attributeNumber < numberOfAttributes; attributeNumber++)
                    {
                        // Get the asn1 encoded attribute name
                        var asn1_attributeName = (Asn1OctetString)asn1_attributeList[attributeNumber];
                        if (asn1_attributeName == null)
                            throw new IOException("Decoding error");

                        // Get attributename string
                        ReplicationFilter[classNumber][attributeNumber + 1] = asn1_attributeName.StringValue;
                        if (ReplicationFilter[classNumber][attributeNumber + 1] == null)
                            throw new IOException("Decoding error");
                    }
                }
            }
        }
    }
}