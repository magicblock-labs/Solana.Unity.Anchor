using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using Solana.Unity.Rpc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Anchor.Client
{
    public class ResultWrapper<T>
    {
        public ResultWrapper(RequestResult<ResponseValue<AccountInfo>> result)
        {
            OriginalRequest = result;
        }

        public ResultWrapper(RequestResult<ResponseValue<AccountInfo>> result, T parsedResult)
        {
            OriginalRequest = result;
            ParsedResult = parsedResult;
        }

        public RequestResult<ResponseValue<AccountInfo>> OriginalRequest { get; init; }

        public T ParsedResult { get; set; }

        public bool WasDeserializationSuccessful => ParsedResult != null;

        public bool WasSuccessful => OriginalRequest.WasSuccessful && WasDeserializationSuccessful;

    }
}
