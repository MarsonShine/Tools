using System;
using System.Reflection;

namespace FakeApi {
    public class FakeGenerator {
        public void Analyze<T>(T instance) {
            var t = instance.GetType();
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties) {

            }
        }
    }
}