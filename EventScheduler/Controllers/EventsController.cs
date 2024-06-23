using EventScheduler.Common.Helpers;
using EventScheduler.Data;
using EventScheduler.Helpers;
using EventScheduler.Models.Entities;
using EventScheduler.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Util;
using System.Globalization;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventScheduler.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public IActionResult EventsCalendar()
        {
            return PartialView("EventsCalendar");
        }

        public async Task<JsonResult> GetEvents()
        {
            try
            {
                var events = await _eventRepository.GetEventsAsync();
                var eventList = events.Select(e => new {
                    id = e.EventId,
                    title = e.EventTitle,
                    start = e.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = e.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    description = e.EventDescription,
                    color = e.Priority == Enums.Priority.High ? "green" :
                            e.Priority == Enums.Priority.Medium ? "orange" :
                            "blue"
                }).ToList();
                return new JsonResult(eventList);
            }
            catch (Exception ex)
            {
                // Handle parsing or other exceptions
                Console.WriteLine("Error parsing date: " + ex.Message);
                return new JsonResult(ex);
            }
        }

        public IActionResult AddEvent()
        {
            var model = new Event();
            return PartialView("_AddEditEventPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddEventAsync(model);
                ViewBag.CloseAlert = "<script>$(document).ready(function () {closeParent();});</script>";
            }
            return PartialView("_AddEditEventPartial", model);
        }

        public async Task<IActionResult> EditEvent(int id)
        {
            var oEvent = await _eventRepository.GetEventByIdAsync(id);
            if (oEvent == null)
            {
                return NotFound();
            }
            return PartialView("_AddEditEventPartial", oEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.UpdateEventAsync(model);
                ViewBag.CloseAlert = "<script>$(document).ready(function () {closeParent();});</script>";
            }
            return PartialView("_AddEditEventPartial", model);
        }

        // POST: /Events/UpdateEventDate
        [HttpPost]
        public async Task<IActionResult> UpdateEventDate([FromBody] EventDateUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var eventToUpdate = await _eventRepository.GetEventByIdAsync(model.Id);
                    if (eventToUpdate == null)
                    {
                        return NotFound();
                    }

                    SimpleDateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'");
                    SimpleDateFormat outputFormat = new SimpleDateFormat("yyyy-MM-dd hh:mm:ss a");
                    eventToUpdate.StartDate = inputFormat.Parse(model.Start);
                    eventToUpdate.EndDate = inputFormat.Parse(model.End);
                    await _eventRepository.UpdateEventAsync(eventToUpdate);

                    return Ok();
                }
                catch (Exception ex)
                {
                    // Handle parsing or other exceptions
                    Console.WriteLine("Error parsing date: " + ex.Message);
                    return BadRequest("Invalid date format.");
                }
            }

            return BadRequest(ModelState);
        }
    }
}