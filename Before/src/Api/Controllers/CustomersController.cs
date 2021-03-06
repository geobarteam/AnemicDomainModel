﻿using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dtos;
using Logic.Entities;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;

        public CustomersController(UnitOfWork unitOfWork, MovieRepository movieRepository, CustomerRepository customerRepository)
            : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            var dto = new CustomerDto
            {
                Id = customer.Id,
                Email = customer.Email.Value,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate,
                PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
                {
                    Price = x.Price,
                    ExpirationDate = x.ExpirationDate,
                    PurchaseDate = x.PurchaseDate,
                    Movie = new MovieDto
                    {
                        Name = x.Movie.Name
                    }
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            IReadOnlyList<Customer> customers = _customerRepository.GetList();
            List<CustomerInListDto> dto = customers.Select(n => new CustomerInListDto
            {
                Email = n.Email.Value,
                Id = n.Id,
                MoneySpent = n.MoneySpent,
                StatusExpirationDate = n.Status.ExpirationDate,
                Status = n.Status.ToString(),
                Name = n.Name.Value
            }).ToList();

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            
                Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
                Result<Email> emailOrError = Email.Create(item.Email);

                Result result = Result.Combine(customerNameOrError, emailOrError);

                if (result.IsFailure)
                {
                    return Error(result.Error);
                }

                if (_customerRepository.GetByEmail(emailOrError.Value) != null)
                {
                    return Error("Email is allready in use:" + emailOrError.Value);
                }

                var customer = new Customer(customerNameOrError.Value, emailOrError.Value);
                _customerRepository.Add(customer);

                return Ok();
            
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
        {
            
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return Error("Invalid customer id: " + id);
                }

                customer.Name = CustomerName.Create(item.Name).Value;

                return Ok();
            
        }

        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            
                Movie movie = _movieRepository.GetById(movieId);
                if (movie == null)
                {
                    return Error("Invalid movie id: " + movieId);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return Error("Invalid customer id: " + id);
                }

                if (customer.PurchasedMovies.Any(x => x.Movie.Id == movie.Id && !x.ExpirationDate.IsExpired))
                {
                    return Error("The movie is already purchased: " + movie.Name);
                }

                customer.PurchaseMovie(movie);

                return Ok();
           
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            
                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return Error("Invalid customer id: " + id);
                }

                var success = customer.CanPromote();
                if (success.IsFailure)
                {
                    return Error(success.Error);
                }
                
                customer.Promote();

                return Ok();
            
        }
    }
}
