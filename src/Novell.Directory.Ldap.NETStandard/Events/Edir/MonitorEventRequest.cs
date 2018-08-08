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
// Novell.Directory.Ldap.Events.Edir.MonitorEventRequest.cs
//
// Author:
//   Anil Bhatia (banil@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using System.IO;
using Novell.Directory.Ldap.Asn1;
using Novell.Directory.Ldap.Utilclass;

namespace Novell.Directory.Ldap.Events.Edir
{
    /// <summary>
    ///     This class denotes the mechanism to specify the event of interest.
    /// </summary>
    public class MonitorEventRequest : LdapExtendedOperation
    {
        static MonitorEventRequest()
        {
            /*
             * Register the extendedresponse class which is returned by the
             * server in response to a MonitorEventRequest
             */
            LdapExtendedResponse.Register(EventOids.NldapMonitorEventsResponse, typeof(MonitorEventResponse));

            // Also try to register EdirEventIntermediateResponse
            LdapIntermediateResponse.Register(EventOids.NldapEventNotification, typeof(EdirEventIntermediateResponse));
        } // end of static constructor

        public MonitorEventRequest(EdirEventSpecifier[] specifiers)
            : base(EventOids.NldapMonitorEventsRequest, null)
        {
            if (specifiers == null)
            {
                throw new ArgumentException(ExceptionMessages.ParamError);
            }

            var encodedData = new MemoryStream();
            var encoder = new LberEncoder();

            var asnSequence = new Asn1Sequence();
            try
            {
                asnSequence.Add(new Asn1Integer(specifiers.Length));

                var asnSet = new Asn1Set();
                var bFiltered = false;
                for (var nIndex = 0; nIndex < specifiers.Length; nIndex++)
                {
                    var specifierSequence = new Asn1Sequence();
                    specifierSequence.Add(new Asn1Integer((int)specifiers[nIndex].EventType));
                    specifierSequence.Add(new Asn1Enumerated((int)specifiers[nIndex].EventResultType));
                    if (nIndex == 0)
                    {
                        bFiltered = specifiers[nIndex].EventFilter != null;
                        if (bFiltered)
                        {
                            SetId(EventOids.NldapFilteredMonitorEventsRequest);
                        }
                    }

                    if (bFiltered)
                    {
                        // A filter is expected
                        if (specifiers[nIndex].EventFilter == null)
                        {
                            throw new ArgumentException("Filter cannot be null,for Filter events");
                        }

                        specifierSequence.Add(new Asn1OctetString(specifiers[nIndex].EventFilter));
                    }
                    else
                    {
                        // No filter is expected
                        if (specifiers[nIndex].EventFilter != null)
                        {
                            throw new ArgumentException("Filter cannot be specified for non Filter events");
                        }
                    }

                    asnSet.Add(specifierSequence);
                }

                asnSequence.Add(asnSet);
                asnSequence.Encode(encoder, encodedData);
            }
            catch (Exception e)
            {
                throw new LdapException(
                    ExceptionMessages.EncodingError,
                    LdapResultCode.EncodingError,
                    null, e);
            }

            SetValue(encodedData.ToArray());
        } // end of the constructor MonitorEventRequest
    }
}