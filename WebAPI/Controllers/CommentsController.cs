using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using BLL.Interfaces;
using BLL.Exceptions;
using BLL.Models.FormsToFill;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase, IDisposable
    {
        private readonly ICommentService _commentService;
        private bool _disposed;


        public CommentsController(ICommentService service)
        {
            _commentService = service;
            _disposed = false;
        }

        // GET: api/Posts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CommentModel>>> GetComment()
        //{
        //    return Ok(await _commentService.GetAllAsync().ConfigureAwait(false));
        //}

        //// GET: api/Posts/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CommentModel>> GetComment(int id)
        //{
        //    var post = await _commentService.GetAsync(id).ConfigureAwait(false);

        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return post;
        //}



        // POST: api/Posts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentModel>> PostComment([FromBody]NewCommentModel model)
        {
            if (model == null)
                return BadRequest();

            int commentId = 0;
            try
            {
                commentId = await _commentService.NewComment(model).ConfigureAwait(false);
            }
            catch (AccessException ex)
            {
                return Forbid(ex.Message);
            }

            return Ok(commentId);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<CommentModel>> DeleteComment(int id)
        {
            try
            {
                await _commentService.DeleteComment(id).ConfigureAwait(false);
            }
            catch (DataException ex)
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
                    _commentService.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
