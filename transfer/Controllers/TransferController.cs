using Microsoft.AspNetCore.Mvc;
using transfer.Core.Transfer;
using transfer.Core.Transfer.Interface;
using transfer.Exceptions;

namespace transfer.Controllers
{
    [ApiController]
    [Route("api/fund-transfer")]
    public class TransferController : ControllerBase
    {
        public TransferController(ITransfer transfer)
        {
            _transfer = transfer;
        }

        private readonly ITransfer _transfer;

        [HttpGet]
        [Route("{transactionId}")]
        public ActionResult<TransferDto> Index(int transactionId)
        {
            try
            {
                return Ok(_transfer.Index(transactionId));
            }
            catch (InvalidTransactionId ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<dynamic> Create([FromBody] TransferRequest transferRequest)
        {
            try
            {
                return Ok( new { transactionId = _transfer.Create(transferRequest) });
            }
            catch (InvalidValue ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
        }
    }
}
