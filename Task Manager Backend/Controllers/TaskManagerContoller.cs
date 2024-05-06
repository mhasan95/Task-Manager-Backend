using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Task_Manager_Backend.Models;
using Serilog;

namespace Task_Manager_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskManagerController : ControllerBase
    {

        private readonly ILogger<TaskManagerController> _logger;

        public TaskManagerController(ILogger<TaskManagerController> logger)
        {
            _logger = logger;
        }

    }
}
