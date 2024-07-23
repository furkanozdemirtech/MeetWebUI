using CreatedMeetWebUI.ApiJobs;
using CreatedMeetWebUI.Extensions;
using CreatedMeetWebUI.Models;
using CreatedMeetWebUI.Tools;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreatedMeetWebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MeetController : Controller
    {

        public object Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44359/Meet/GetAll";
            var apiLoads = ApiLoads<MEET>.GetInstance(apiUrl);
            var loadTask = apiLoads.LoadtListAsync();
            loadTask.Wait();
            var listdata = apiLoads.List.InitializeIfNull();
            return DataSourceLoader.Load(listdata, loadOptions);
        }
        [HttpGet]
        [Route("/Meet/Create")]
        public IActionResult Create()
        {
            var model = new MEET();
            return View(model);
        }
        [HttpGet]
        [Route("/Meet/AssignParticipants")]
        public IActionResult AssignParticipants()
        {
            var item = new MEET();
            item.ID = 1;
            var apiUrl = "https://localhost:44359/Meet/GetItem";
            var apiloads = ApiLoads<MEET>.GetInstance(apiUrl);
            var loadTask = apiloads.PostAsync(apiUrl, item);
            loadTask.Wait();
            HalfandStaticObject.GetInstance().SelectedRegistry.Meeting = apiloads.List[0];
            return View(HalfandStaticObject.GetInstance().SelectedRegistry);
        }
        [HttpPost]
        [Route("/Meet/AssignParticipants")]
        public IActionResult AssignParticipants(MeetingViewModel meetingView)
        {
            var item = new MEET();
            item.ID = 1;
            var apiUrl = "https://localhost:44359/Meet/GetItem";
            var apiloads = ApiLoads<MEET>.GetInstance(apiUrl);
            var loadTask = apiloads.PostAsync(apiUrl, item);
            loadTask.Wait();
            HalfandStaticObject.GetInstance().SelectedRegistry.Meeting = apiloads.List[0];
            return View(HalfandStaticObject.GetInstance().SelectedRegistry);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] MEET model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var apiUrl = "https://localhost:44359/Meet/MeetPost?";
                var apiLoads = ApiLoads<MEET>.GetInstance(apiUrl);
                var result = await apiLoads.PostAsync(apiUrl, model);

                if (!result.IsSuccessStatusCode)
                {
                    return Ok("Meet created successfully");
                }
                else // Başarısız ise
                {
                    return BadRequest("Failed to create meet"); // Hata mesajı döndür
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda isteği işle
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("/Meet/Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                // Find the MEET entity to delete
                //var meet = _context.Meets.Find(id);
                //if (meet == null)
                //{
                //    return NotFound("Meet not found");
                //}

                //// Remove the entity from the context
                //_context.Meets.Remove(meet);
                //_context.SaveChanges();
                var model = new MEET();
                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
