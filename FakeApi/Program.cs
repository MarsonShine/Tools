using System;
using FakeApi.Model;

namespace FakeApi {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            FakeGenerator fg = new FakeGenerator();
            fg.Analyze<ComplexModel>();
        }
    }
}