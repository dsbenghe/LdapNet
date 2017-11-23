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
// Novell.Directory.Ldap.Events.Edir.EventData.DSETimeStamp.cs
//
// Author:
//   Anil Bhatia (banil@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System.Text;
using Novell.Directory.Ldap.Asn1;

namespace Novell.Directory.Ldap.Events.Edir
{
    /// <summary>
    ///     The class represents the Timestamp datastructure for Edir events
    ///     Notification.
    /// </summary>
    public class DSETimeStamp
    {
        public int Seconds { get; protected set; }
        public int ReplicaNumber { get; protected set; }
        public int Event { get; protected set; }

        public DSETimeStamp(Asn1Sequence dseObject)
        {
            Seconds = ((Asn1Integer)dseObject[0]).IntValue;
            ReplicaNumber = ((Asn1Integer)dseObject[1]).IntValue;
            Event = ((Asn1Integer)dseObject[2]).IntValue;
        }

        /// <summary>
        ///     Returns a string representation of the object.
        /// </summary>
        public override string ToString()
        {
            var buf = new StringBuilder();

            buf.AppendFormat("[TimeStamp (seconds={0})", Seconds);
            buf.AppendFormat("(replicaNumber={0})", ReplicaNumber);
            buf.AppendFormat("(event={0})", Event);
            buf.Append("]");

            return buf.ToString();
        }
    }
}