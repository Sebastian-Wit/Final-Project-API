using System;
using System.ComponentModel.DataAnnotations;
namespace MyFirstAPI.Models
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string? StatusDescription { get; set; }
        public T? Data { get; set; }

    }
}