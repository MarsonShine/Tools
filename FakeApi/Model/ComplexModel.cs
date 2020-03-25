using System;
using System.Collections.Generic;

namespace FakeApi.Model {
    public class ComplexModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public long Lid { get; set; }
        public StatusEnum Status { get; set; }
        public List<SimpleModel> Simples { get; set; }
        public string[] GradeIds { get; set; }
        public DateTime? UpdationTime { get; set; }
        public SimpleModel[] SimpleArray { get; set; }

    }

    public enum StatusEnum {
        Enabled,
        Disabled
    }
}