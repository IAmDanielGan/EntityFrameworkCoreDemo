using BookStoreWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            using(var context=new BookStoresDbContext())
            {
                return context.Authors.ToList();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IEnumerable<Author> GetAuthorByID(int id)
        {
            using(var context=new BookStoresDbContext())
            {
                return context.Authors.Where(auth => auth.AuthorId == id).ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Author>  AddAuthor()
        {
            using(var context=new BookStoresDbContext())
            {
                Author newAuthor = new Author();
                newAuthor.FirstName = "Daniel" + new Random().Next(1, 1000);
                newAuthor.LastName = "Gan" + new Random().Next(1, 1000);
                context.Authors.Add(newAuthor);
                context.SaveChanges();
                return context.Authors.Where(auth => auth.FirstName == newAuthor.FirstName).ToList();
            }
        }

        [HttpDelete]
        public void DeleteAuthorByID(int id)
        {
            using(var context= new BookStoresDbContext())
            {
                Author auth = context.Authors.Where(au => au.AuthorId == id).FirstOrDefault();
                if(auth!=null)
                {
                    context.Authors.Remove(auth);
                    context.SaveChanges();
                }
            }
        }
    }
}
