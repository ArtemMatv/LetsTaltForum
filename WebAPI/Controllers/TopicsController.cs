using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.FormsToFill;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase, IDisposable
    {
        private readonly ITopicService _topicService;
        private bool _disposed;
        public TopicsController(ITopicService service)
        {
            _topicService = service;
            _disposed = false;
        }

        // GET: api/Topics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicModel>>> GetTopic()
        {
            var topics = await _topicService.GetAllAsync().ConfigureAwait(false);
            return Ok(topics);
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicModel>> GetTopic(int id)
        {
            var topic = await _topicService.GetAsync(id).ConfigureAwait(false);

            if (topic == null)
            {
                return NotFound();
            }

            return topic;
        }


        // POST: api/Topics
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TopicModel>> PostTopic([FromBody]NewTopicModel model)
        {
            if (model == null)
                return BadRequest();

            if (model.AuthorRole.Name != "Admin")
                return Forbid();

            await _topicService.CreateTopic(model).ConfigureAwait(false);

            return Ok();
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TopicModel>> DeleteTopic(int id)
        {
            try
            {
                await _topicService.DeleteTopic(id).ConfigureAwait(false);
            }
            catch (ArgumentException ex)
            {
                NotFound(ex.Message);
            }

            return Ok();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _topicService.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
