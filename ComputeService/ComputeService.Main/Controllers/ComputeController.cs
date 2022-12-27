using DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace ComputeService.Main.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ComputeController : Controller, IComputeApi
    {
        [HttpPost]
        public Task<int> GetArraySum(ArrayMessage request)
        {
            Console.WriteLine("Считаем массив");
            Console.WriteLine(request.Index);
            return Task.FromResult(request.Array.Sum());
        }

    }
}
