using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DataContracts
{
    public interface IComputeApi
    {
        [Post("/Compute/GetArraySum")]
        Task<int> GetArraySum(
           [Body] ArrayMessage arrayMessage
        );

    }
}
