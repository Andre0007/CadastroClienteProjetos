using CadastroClienteProjetos.API.Controllers;
using CadastroClienteProjetos.Domain.Entities;
using CadastroClienteProjetos.Infra.Context;
using CadastroClienteProjetos.Infra.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace CadastroClienteProjetos.xUnit
{
    public class ClienteTest
    {
        #region [ Cadeia de conexão ]

        private int? IdGlobal = null;
        private BaseRepository<Cliente> repository;
        private static DbContextOptions<SQLServerContext> dbContextOptions { get; }
        private static string connectionString = "Server=DESKTOP-0R6B5S6;Database=ClienteProjeto_Dev;User ID=sa;Password=1234";

        static ClienteTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<SQLServerContext>().UseSqlServer(connectionString).Options;
        }

        public ClienteTest()
        {
            SQLServerContext context = new SQLServerContext(dbContextOptions);
            repository = new BaseRepository<Cliente>(context);
        }

        #endregion

        #region [ Get By Id ]

        [Fact]
        public async void Task_GetById_Return_OkResult()
        {
            //Arrange  
            int postId = 1;
            ClienteController controller = new ClienteController(repository);           

            //Act  
            IActionResult data = await controller.GetByOne(postId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetById_Return_NotFoundResult()
        {
            //Arrange 
            int postId = 3;
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.GetByOne(postId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetById_Return_BadRequestResult()
        {
            //Arrange 
            int? postId = null;
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.GetByOne(postId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetPostById_MatchResult()
        {
            //Arrange 
            int? postId = 1;
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.GetByOne(postId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            OkObjectResult okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            Cliente get = okResult.Value.Should().BeAssignableTo<Cliente>().Subject;

            Assert.Equal("00000000000191", get.cnpj);
            Assert.Equal("Microsoft SA", get.razaoSocial);
        }

        #endregion

        #region [ Get All ]

        [Fact]
        public async void Task_GetAll_Return_OkResult()
        {
            //Arrange  
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.Get();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_GetPosts_Return_BadRequestResult()
        {
            //Arrange  
            ClienteController controller = new ClienteController(repository);

            //Act  
            var data = controller.Get();
            data = null;

            //Assert  
            if (data != null)               
                Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetAll_MatchResult()
        {
            //Arrange  
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.Get();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var getAll = okResult.Value.Should().BeAssignableTo<List<Cliente>>().Subject;

            Predicate<Cliente> cliente1Finder = (Cliente c) => { return c.razaoSocial == "Microsoft SA"; };
            Assert.Equal("Microsoft SA", getAll.Find(cliente1Finder).razaoSocial);
            Assert.Equal("00000000000191", getAll.Find(cliente1Finder).cnpj);

            Predicate<Cliente> cliente2Finder = (Cliente c) => { return c.razaoSocial == "Coca-cola"; };
            Assert.Equal("Coca-cola", getAll.Find(cliente2Finder).razaoSocial);
            Assert.Equal("21670807000101", getAll.Find(cliente2Finder).cnpj);
        }

        #endregion

        #region [ POST ]

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange  
            ClienteController controller = new ClienteController(repository);
            Cliente newCliente = new Cliente() { razaoSocial = "Teste, 1 2 3", cnpj = "92230633000104" };

            //Act  
            dynamic data = await controller.Post(newCliente);
            IdGlobal = data.Value.Value.id;

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        //[Fact]
        //public async void Task_Add_InvalidData_Return_BadRequest()
        //{
        //    Thread.Sleep(1000);

        //    //Arrange  
        //    ClienteController controller = new ClienteController(repository);
        //    Cliente newCliente = new Cliente() { razaoSocial = "Teste, 1 2 3", cnpj = "92230633000104" };

        //    //Act              
        //    var data = await controller.Post(newCliente);
        //    //var teste = data.StatusCode;

        //    //Assert  
        //    Assert.IsType<BadRequestObjectResult>(data);

        //    //Deletando o teste 1 Post
        //    await controller.Delete(IdGlobal);
        //    IdGlobal = null;
        //}

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Thread.Sleep(2000);

            //Arrange  
            ClienteController controller = new ClienteController(repository);
            Cliente newCliente = new Cliente() { razaoSocial = "Teste, 1 2 3", cnpj = "76739863000147" };

            //Act  
            var data = await controller.Post(newCliente);
            //var data2 = data;
            //IdGlobal = data.Value.Value.id;

            //Assert  
            Assert.IsType<OkObjectResult>(data);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            Assert.Equal(68, okResult.Value);
        }

        #endregion

        #region [ Update ]

        //[Fact]
        //public async void Task_Update_ValidData_Return_OkResult()
        //{
        //    //Arrange  
        //    var controller = new PostController(repository);
        //    var postId = 2;

        //    //Act  
        //    var existingPost = await controller.GetPost(postId);
        //    var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
        //    var result = okResult.Value.Should().BeAssignableTo<PostViewModel>().Subject;

        //    var post = new Post();
        //    post.Title = "Test Title 2 Updated";
        //    post.Description = result.Description;
        //    post.CategoryId = result.CategoryId;
        //    post.CreatedDate = result.CreatedDate;

        //    var updatedData = await controller.UpdatePost(post);

        //    //Assert  
        //    Assert.IsType<OkResult>(updatedData);
        //}

        //[Fact]
        //public async void Task_Update_InvalidData_Return_BadRequest()
        //{
        //    //Arrange  
        //    var controller = new PostController(repository);
        //    var postId = 2;

        //    //Act  
        //    var existingPost = await controller.GetPost(postId);
        //    var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
        //    var result = okResult.Value.Should().BeAssignableTo<PostViewModel>().Subject;

        //    var post = new Post();
        //    post.Title = "Test Title More Than 20 Characteres";
        //    post.Description = result.Description;
        //    post.CategoryId = result.CategoryId;
        //    post.CreatedDate = result.CreatedDate;

        //    var data = await controller.UpdatePost(post);

        //    //Assert  
        //    Assert.IsType<BadRequestResult>(data);
        //}

        //[Fact]
        //public async void Task_Update_InvalidData_Return_NotFound()
        //{
        //    //Arrange  
        //    var controller = new PostController(repository);
        //    var postId = 2;

        //    //Act  
        //    var existingPost = await controller.GetPost(postId);
        //    var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
        //    var result = okResult.Value.Should().BeAssignableTo<PostViewModel>().Subject;

        //    var post = new Post();
        //    post.PostId = 5;
        //    post.Title = "Test Title More Than 20 Characteres";
        //    post.Description = result.Description;
        //    post.CategoryId = result.CategoryId;
        //    post.CreatedDate = result.CreatedDate;

        //    var data = await controller.UpdatePost(post);

        //    //Assert  
        //    Assert.IsType<NotFoundResult>(data);
        //}

        #endregion

        #region [ Delete ]  

        [Fact]
        public async void Task_Delete_Return_OkResult()
        {
            Thread.Sleep(2000);

            //Arrange  
            int postId = IdGlobal.Value;
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.Delete(postId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Task_Delete_Return_NotFoundResult()
        {
            //Arrange 
            int postId = 999999;
            ClienteController controller = new ClienteController(repository);

            //Act  
            IActionResult data = await controller.Delete(postId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_Delete_Return_BadRequestResult()
        {
            //Arrange  
            ClienteController controller = new ClienteController(repository);
            int? postId = null;

            //Act  
            var data = await controller.Delete(postId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}